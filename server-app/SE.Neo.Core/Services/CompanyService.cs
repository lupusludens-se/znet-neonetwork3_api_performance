using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SE.Neo.Common.Attributes;
using SE.Neo.Common.Enums;
using SE.Neo.Common.Models;
using SE.Neo.Common.Models.CMS;
using SE.Neo.Common.Models.Company;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Common.Models.UserProfile;
using SE.Neo.Core.Configs;
using SE.Neo.Core.Constants;
using SE.Neo.Core.Data;
using SE.Neo.Core.Entities;
using SE.Neo.Core.Enums;
using SE.Neo.Core.Services.Interfaces;
using UserStatus = SE.Neo.Core.Enums.UserStatus;

namespace SE.Neo.Core.Services
{
    public partial class CompanyService : BaseService, ICompanyService
    {
        private readonly ApplicationContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<CompanyService> _logger;
        private readonly MemoryCacheTimeStamp _memoryCacheTimeStamp;

        public CompanyService(
            ApplicationContext context,
            ILogger<CompanyService> logger,
            IMapper mapper,
            IOptions<MemoryCacheTimeStamp> memoryCacheTimeStamp,
            IDistributedCache cache) : base(cache)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _memoryCacheTimeStamp = memoryCacheTimeStamp.Value;
        }

        public async Task<WrapperModel<CompanyDTO>> GetCompaniesAsync(BaseSearchFilterModel filter, int? userId = null)
        {
            var companyQueryable = ExpandSortCompanies(_context.Companies.AsNoTracking(), filter.Expand, filter.OrderBy);

            companyQueryable = FilterSearchCompanies(companyQueryable, filter.Search, filter.FilterBy);
            if (filter.Random.HasValue)
            {
                companyQueryable = companyQueryable.OrderBy(_ => Guid.NewGuid()).Take(filter.Random.Value);
            }
            int count = 0;
            if (filter.IncludeCount)
            {
                count = await companyQueryable.CountAsync();
                if (count == 0)
                {
                    return new WrapperModel<CompanyDTO> { Count = count, DataList = new List<CompanyDTO>() };
                }
            }

            if (filter.Skip.HasValue)
            {
                companyQueryable = companyQueryable.Skip(filter.Skip.Value);
            }

            if (filter.Take.HasValue)
            {
                companyQueryable = companyQueryable.Take(filter.Take.Value);
            }

            IEnumerable<Company> companies = await companyQueryable.ToListAsync();
            List<CompanyDTO> companiesDTOs = companies.Select(_mapper.Map<CompanyDTO>).ToList();

            if (!string.IsNullOrEmpty(filter.Expand))
            {
                string expand = filter.Expand.ToLower();
                if (expand.Contains("followers"))
                {
                    List<CompanyFollower> companyFollowers =
                        await _context.CompanyFollowers.Where(f => companiesDTOs.Select(cdto => cdto.Id).Contains(f.CompanyId)).AsNoTracking().ToListAsync();
                    for (int i = 0; i < companiesDTOs.Count; i++)
                    {
                        IEnumerable<CompanyFollower> followers = companyFollowers.Where(f => f.CompanyId.Equals(companiesDTOs[i].Id));
                        companiesDTOs[i].FollowersCount = followers.Count();

                        if (userId.HasValue)
                            companiesDTOs[i].IsFollowed = followers.Any(df => df.FollowerId == userId);
                    }
                }
            }
            return new WrapperModel<CompanyDTO> { Count = count, DataList = companiesDTOs };
        }

        public async Task<CompanyDTO?> GetCompanyAsync(int id, int? userId = null, string? expand = null)
        {
            var companiesQueryable = ExpandSortCompanies(_context.Companies.AsNoTracking(), expand);
            var company = await companiesQueryable.FirstOrDefaultAsync(p => p.Id == id && p.StatusId != Enums.CompanyStatus.Deleted);
            var companyDTO = _mapper.Map<CompanyDTO>(company);

            if (!string.IsNullOrEmpty(expand) && companyDTO != null)
            {
                expand = expand.ToLower();
                if (expand.Contains("followers"))
                {
                    IQueryable<CompanyFollower> followersQueryable = _context.CompanyFollowers.Include(f => f.Follower)
                        .Where(f => f.CompanyId.Equals(id) && f.Follower.StatusId == UserStatus.Active)
                        .AsNoTracking();

                    if (expand.Contains("followers.user.roles"))
                    {
                        followersQueryable = followersQueryable.Include(f => f.Follower)
                            .ThenInclude(u => u.Roles)
                            .ThenInclude(r => r.Role);
                    }
                    if (expand.Contains("followers.user.roles.images.company"))
                    {
                        followersQueryable = followersQueryable.Include(f => f.Follower)
                            .ThenInclude(u => u.Company).Include(v => v.Follower).ThenInclude(v => v.Image).Include(v => v.Follower).ThenInclude(v => v.UserProfile);
                    }
                    HashSet<int> followedIds = new HashSet<int>(
                        await _context.UserFollowers.Where(f => f.FollowerId.Equals(userId)).Select(f => f.FollowedId).ToListAsync());
                    IEnumerable<CompanyFollower> followers = await followersQueryable.OrderBy(x => x.Follower.FirstName).ThenBy(x => x.Follower.LastName).ToListAsync();
                    companyDTO.FollowersCount = followers.Count();
                    companyDTO.Followers = followers.Select(f =>
                    {
                        var followerDTO = _mapper.Map<CompanyFollowerDTO>(f); 
                        if (followerDTO.Follower != null)
                        {
                            followerDTO.Follower.IsFollowed = followedIds.Contains(followerDTO.FollowerId);
                        }
                        return followerDTO;
                    }).ToList();


                    if (userId.HasValue)
                        companyDTO.IsFollowed = followers.Any(df => df.FollowerId == userId);
                }
            }
            return companyDTO;
        }

        public async Task<CompanyDTO> CreateUpdateCompanyAsync(int id, CompanyDTO modelDTO)
        {
            bool isEdit = id > 0;
            var model = new Company();
            if (isEdit)
            {
                model = _context.Companies.SingleOrDefault(b => b.Id == id);
                if (model == null)
                    throw new CustomException($"{CoreErrorMessages.ErrorOnSaving} Company does not exist.");
            }
            int companyId = isEdit ? id : 0;
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _mapper.Map(modelDTO, model);
                    model.Id = companyId;

                    if (isEdit)
                    {
                        // Update User Status and Project Status if company is Deleted or InActive
                        UpdateDependentEntitiesByCompany(model);
                    }
                    else
                    {
                        _context.Companies.Add(model);
                        _context.SaveChanges();
                        companyId = model.Id;
                    }
                    //    Handle CMS dependencies
                    _context.RemoveRange(_context.CompanyCategories.Where(a => a.CompanyId == companyId));
                    _context.CompanyCategories.AddRange(modelDTO.Categories.Select(item => new CompanyCategory() { CompanyId = companyId, CategoryId = item.Id }));

                    //Handle Url Links
                    _context.RemoveRange(_context.CompanyUrlLinks.Where(a => a.CompanyId == companyId));
                    _context.CompanyUrlLinks.AddRange(modelDTO.UrlLinks.Select(item => new CompanyUrlLink() { CompanyId = companyId, UrlLink = item.UrlLink, UrlName = item.UrlName }));
                    //    Handle OffsitePPAs dependencies
                    _context.RemoveRange(_context.CompanyOffsitePPAs.Where(a => a.CompanyId == companyId));
                    _context.CompanyOffsitePPAs.AddRange(modelDTO.OffsitePPAs.Select(item => new CompanyOffsitePPA() { CompanyId = companyId, OffsitePPAId = (Enums.OffsitePPAs)item.Id }));

                    await _context.SaveChangesAsync();
                    transaction.Commit();

                    _cache.Remove(CoreCacheKeys.CompanyContext);

                    if (model.Users != null)
                    {
                        // ensure users will need to be retiried with updated status for their next request
                        foreach (var user in model.Users)
                        {
                            _cache.Remove(user.Username);
                        }
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new CustomException(CoreErrorMessages.ErrorOnSaving, ex);
                }
            }

            var companyDTO = _mapper.Map<CompanyDTO>(model);
            return companyDTO;
        }

        public async Task<CompanyDTO> PatchCompanyAsync(int id, JsonPatchDocument patchDoc)
        {
            Company? company = await _context.Companies.SingleOrDefaultAsync(t => t.Id == id);
            if (company != null)
            {
                try
                {
                    patchDoc.ApplyTo(company);
                    // Update User Status and Project Status if company is Deleted or InActive
                    UpdateDependentEntitiesByCompany(company);

                    _context.SaveChanges();

                    if (company.Users != null)
                    {
                        // ensure users will need to be retiried with updated status for their next requests
                        foreach (var user in company.Users)
                        {
                            _cache.Remove(user.Username);
                        }
                    }

                    var companyDTO = _mapper.Map<CompanyDTO>(company);
                    return companyDTO;
                }
                catch (Exception)
                {
                    throw new CustomException(CoreErrorMessages.ErrorOnSaving);
                }
            }
            else
            {
                throw new CustomException(CoreErrorMessages.EntityNotFound);
            }
        }

        public async Task<IEnumerable<BaseIdNameDTO>> GetIndustriesAsync()
        {
            List<Industry> industries = await _context.Industries.AsNoTracking().ToListAsync();
            return industries.Select(_mapper.Map<BaseIdNameDTO>);
        }

        public async Task<IEnumerable<BaseIdNameDTO>> GetOffsitePPAsAsync()
        {
            List<OffsitePPA> offsiteppas = await _context.OffsitePPAs.AsNoTracking().ToListAsync();
            return offsiteppas.Select(_mapper.Map<BaseIdNameDTO>);
        }

        public async Task CreateCompanyFollowerAsync(int followerId, int companyId)
        {
            if (_context.Companies.SingleOrDefault(b => b.Id == companyId) == null)
                throw new CustomException($"{CoreErrorMessages.ErrorOnSaving} Company does not exist.");

            if (_context.CompanyFollowers
                .Where(i => i.CompanyId.Equals(companyId) && i.FollowerId.Equals(followerId)).Any())
                throw new CustomException(CoreErrorMessages.FollowerExists);

            try
            {
                var model = new CompanyFollower { FollowerId = followerId, CompanyId = companyId };
                _context.CompanyFollowers.Add(model);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new CustomException(CoreErrorMessages.ErrorOnSaving, ex);
            }
        }

        public async Task RemoveCompanyFollowerAsync(int followerId, int companyId)
        {
            if (_context.Companies.SingleOrDefault(b => b.Id == companyId) == null)
                throw new CustomException($"{CoreErrorMessages.ErrorOnSaving} Company does not exist.");

            var model = await _context.CompanyFollowers
                .SingleOrDefaultAsync(sp => sp.FollowerId == followerId && sp.CompanyId == companyId);
            if (model != null)
            {
                try
                {
                    _context.CompanyFollowers.Remove(model);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw new CustomException(CoreErrorMessages.ErrorOnRemoving, ex);
                }
            }
        }

        public async Task<WrapperModel<CompanyDTO>> GetFollowedCompaniesAsync(int userId, PaginationModel filter)
        {
            IQueryable<CompanyFollower> companyFollowersQueryable = _context.CompanyFollowers
                .Where(t => t.FollowerId.Equals(userId) && t.Company.StatusId.Equals(Enums.CompanyStatus.Active)).Include(i => i.Company)
                .ThenInclude(p => p.Image)
                .OrderByDescending(o => o.Id).AsNoTracking().AsQueryable();

            int count = 0;
            if (filter.IncludeCount)
            {
                count = await companyFollowersQueryable.CountAsync();
                if (count == 0)
                {
                    return new WrapperModel<CompanyDTO> { Count = count, DataList = new List<CompanyDTO>() };
                }
            }

            if (filter.Skip.HasValue)
            {
                companyFollowersQueryable = companyFollowersQueryable.Skip(filter.Skip.Value);
            }

            if (filter.Take.HasValue)
            {
                companyFollowersQueryable = companyFollowersQueryable.Take(filter.Take.Value);
            }

            List<Company> companies = await companyFollowersQueryable.Select(c => c.Company).ToListAsync();
            return new WrapperModel<CompanyDTO> { Count = count, DataList = companies.Select(_mapper.Map<CompanyDTO>) };
        }

        public async Task<CompanyDTO?> GetCompanyByName(string companyName)
        {
            Company? company = await _context.Companies.Where(c => c.Name == companyName && c.StatusId == Enums.CompanyStatus.Active).FirstOrDefaultAsync();
            return company == null ? null : _mapper.Map<CompanyDTO>(company);
        }

        public async Task<List<int>> GetCompanyFollowersIdsAsync(int companyId)
        {
            return await _context.CompanyFollowers.Where(cf => cf.CompanyId == companyId).Select(cf => cf.FollowerId).ToListAsync();
        }
        public async Task<IEnumerable<CompanyDomainDTO>> GetCompanyDomains(int companyId)
        {
            List<CompanyDomain> companyDomains = await _context.CompanyDomains.Where(cd => cd.CompanyId == companyId).ToListAsync();
            return companyDomains.Select(_mapper.Map<CompanyDomainDTO>);
        }

        public async Task CreateCompanyDomainAsync(int companyId, string domainName)
        {
            try
            {
                bool IsExist = _context.CompanyDomains.Any(cd => cd.CompanyId == companyId && cd.DomainName == domainName);
                if (!IsExist)
                {
                    CompanyDomain model = new CompanyDomain { CompanyId = companyId, DomainName = domainName, IsActive = false };
                    _context.CompanyDomains.Add(model);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                throw new CustomException(CoreErrorMessages.ErrorOnSaving, ex);
            }
        }

        #region Validation

        public async Task<bool> IsCompanyIdExistAsync(int companyId)
        {
            return await IsIdExistAsync(companyId, _context.Companies, CoreCacheKeys.CompanyContext, _memoryCacheTimeStamp.Medium);
        }

        public async Task<bool> IsIndustryIdExistAsync(int industryId)
        {
            return await IsIdExistAsync(industryId, _context.Industries, CoreCacheKeys.IndustryContext, _memoryCacheTimeStamp.Medium);
        }

        public bool IsCompanyNameUnique(string companyName, bool includeActiveStatusOnly = false, int? companyId = null)
        {
            IQueryable<Company> companyQueryable = _context.Companies.Where(p => p.Name == companyName);

            if (includeActiveStatusOnly)
            {
                companyQueryable = companyQueryable.Where(c => c.StatusId == Enums.CompanyStatus.Active || c.StatusId == Enums.CompanyStatus.Inactive);
            }

            if (companyId != null)
                return !companyQueryable.Where(p => !p.Id.Equals(companyId)).Any();

            return !companyQueryable.Any();
        }

        public bool IsCompanyDomainExist(int companyId, string domainName)
        {
            if (string.IsNullOrEmpty(domainName))
            {
                return false;
            }
            CompanyDomain? companyDomain = _context.CompanyDomains.FirstOrDefault(p => p.CompanyId == companyId && p.DomainName == domainName && p.IsActive);
            return companyDomain != null;
        }
        #endregion Validation


        /// <summary>
        ///   Get list of saved files of a company
        /// </summary>
        /// <param name="companyIdFromQS">companyId From Query String</param>
        /// <param name="filter"></param>
        /// <param name="userId"></param>
        /// <param name="isAdmin"></param>
        /// <param name="isPrivate"></param>
        /// <param name="currentUserCompanyId"></param>
        /// <returns></returns>
        public async Task<WrapperModel<CompanyFileDTO>> GetSavedFilesOfACompanyAsync(int companyIdFromQS, BaseSearchFilterModel filter, int userId, bool isAdmin, bool isPrivate, int currentUserCompanyId)
        {
            IQueryable<CompanyFile> companyFilesQuery = _context.CompanyFile.AsNoTracking().Where(i => i.CompanyId == companyIdFromQS && i.Company.StatusId == Enums.CompanyStatus.Active);

            if (isPrivate)
            {
                companyFilesQuery = companyFilesQuery.Where(x => x.IsPrivate && (isAdmin || (!isAdmin && x.CompanyId == currentUserCompanyId)));
            }
            else
            {
                companyFilesQuery = companyFilesQuery.Where(x => !x.IsPrivate);
            }


            companyFilesQuery = ExpandSortFiles(companyFilesQuery, filter.Expand, filter.OrderBy);

            int count = 0;
            if (filter.IncludeCount)
            {
                count = await companyFilesQuery.CountAsync();
                if (count == 0)
                {
                    return new WrapperModel<CompanyFileDTO> { Count = count, DataList = new List<CompanyFileDTO>() };
                }
            }

            if (filter.Skip.HasValue)
            {
                companyFilesQuery = companyFilesQuery.Skip(filter.Skip.Value);
            }

            if (filter.Take.HasValue)
            {
                companyFilesQuery = companyFilesQuery.Take(filter.Take.Value);
            }


            var companyFiles = companyFilesQuery
               .Join(_context.Users,
                     cf => cf.UpdatedByUserId,
                     u => u.Id,
                     (cf, u) => new CompanyFileDTO()
                     {
                         Id = cf.FileId,
                         ActualFileName = cf.File.ActualFileName ?? string.Empty,
                         ActualFileTitle = cf.File.ActualFileTitle ?? string.Empty,
                         CompanyId = cf.CompanyId,
                         Name = cf.File.Name ?? string.Empty,
                         Type = cf.File.Type,
                         Extension = cf.File.Extension,
                         Link = cf.File.Link,
                         Size = cf.File.Size,
                         Version = cf.File.Version,
                         CreatedOn = Convert.ToDateTime(cf.File.CreatedOn),
                         ModifiedOn = Convert.ToDateTime(cf.File.ModifiedOn),
                         ModifiedBy = $"{u.LastName}, {u.FirstName}"
                     });

            return new WrapperModel<CompanyFileDTO>
            {
                Count = count,
                DataList = companyFiles
            };
        }

        public async Task<string> GetBlobFileName(int fileId)
        {
            var fileDetails = await _context.File.FirstOrDefaultAsync(f => f.Id == fileId);
            return fileDetails?.Name ?? string.Empty;
        }


        /// <summary>
        /// 
        /// </summary>
        /// 
        /// </summary>
        /// <param name="currentUserId"></param>
        /// <param name="currentUserCompanyId"></param>
        /// <param name="fileId"></param>
        /// <param name="IsPrivate"></param>
        /// <param name="isAdmin"></param>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        public async Task<bool> DeleteCompanyFileByTypeAsync(int currentUserId, int currentUserCompanyId, int fileId, bool IsPrivate, bool isAdmin)
        {
            var companyFile = await _context.CompanyFile.FirstOrDefaultAsync(x => x.FileId == fileId && ((x.CompanyId == currentUserCompanyId) || isAdmin));
            if (companyFile != null)
            {
                _context.CompanyFile.Remove(companyFile);
                var file = await _context.File.FirstOrDefaultAsync(f => f.Id == companyFile.FileId);
                if (file != null)
                {
                    _context.File.Remove(file);
                    await _context.SaveChangesAsync();
                    return true;
                }
            }
            throw new Exception(CoreErrorMessages.EntityNotFound);
        }
        /// <param name="companyIdFromQS"></param>
        /// <param name="fileName"></param>
        /// <param name="currentUserId"></param>
        /// <param name="isPrivateFile"></param>
        /// <returns></returns>
        public async Task<FileExistResponseDTO> ValidateFileCountAndIfExistsByCompanyIdAsync(int companyIdFromQS, string fileName, int currentUserId, bool isPrivateFile)
        {
            var fileActualName = fileName.Substring(0, fileName.LastIndexOf("."));
            var fileExtension = (FileExtension)Enum.Parse(typeof(FileExtension), fileName.Substring(fileName.LastIndexOf(".") + 1).ToLower());

            // Here the order by file id descening applied in order to get the latest document uploaded with same name 
            var fileDetails = await _context.File.Join(_context.CompanyFile, file => file.Id, inf => inf.FileId, (file, inf) => new { File = file, CompanyFile = inf })
                .Where(f => f.File.ActualFileTitle != null && f.File.ActualFileTitle == fileActualName &&
                    f.File.Extension == fileExtension && f.CompanyFile.CompanyId == companyIdFromQS && f.CompanyFile.IsPrivate == isPrivateFile)
                .OrderByDescending(f => f.File.Version).FirstOrDefaultAsync();

            var fileExistResponse = new FileExistResponseDTO()
            {
                IsExist = fileDetails != null,
                BlobName = fileDetails?.File?.Name ?? string.Empty,
                ActualFileName = fileDetails?.File?.ActualFileName ?? string.Empty,
                ActualFileTitle = fileDetails?.File?.ActualFileTitle ?? string.Empty,
                FileVersion = fileDetails?.File?.Version ?? 0
            };
            return fileExistResponse;
        }

        /// <summary>
        /// code to save all the company files
        /// </summary>
        /// <param name="companyFileDTO"></param>
        /// <param name="currentUserId"></param>
        /// <param name="companyIdFromQS"></param>
        /// <returns></returns>
        public async Task<bool> UploadFileByCompanyIdAsync(CompanyFileDTO companyFileDTO, int currentUserId, int companyIdFromQS)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (!string.IsNullOrEmpty(companyFileDTO.ActualFileName))
                    {
                        var file = _mapper.Map<Entities.File>(companyFileDTO);
                        var fileEntity = await _context.File.AddAsync(file);
                        await _context.SaveChangesAsync();

                        _context.CompanyFile.Add(new CompanyFile()
                        {
                            CompanyId = companyIdFromQS,
                            FileId = fileEntity.Entity.Id,
                            IsPrivate = companyFileDTO.IsPrivate
                        });
                        await _context.SaveChangesAsync();
                        transaction.Commit();
                        return true;
                    }
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _logger.LogError($"Error while attaching a {companyFileDTO.ActualFileName} file to an company {companyFileDTO.CompanyId}. Error : {ex.Message}");
                }
                return false;
            }
        }

        /// <summary>
        /// Update the file modified date
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileSize"></param>
        /// <returns></returns>
        public async Task<bool> UpdateCompanyFileModifiedDateAndSize(string fileName, int fileSize)
        {
            var file = await _context.File.FirstOrDefaultAsync(x => x.Name == fileName);
            if (file != null)
            {
                _context.Entry(file).Entity.ModifiedOn = DateTime.UtcNow;
                _context.Entry(file).Entity.Size = fileSize;
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                throw new CustomException(CoreErrorMessages.EntityNotFound);
            }
        }
        /// <summary>
        /// Update the file title
        /// </summary>
        /// <param name="fileTitle"></param>
        /// <param name="fileId"></param>
        /// <param name="currentUserId"></param>
        /// <param name="isAdmin"></param>
        /// <param name="currentUserCompanyId"></param>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        public async Task<bool> UpdateFileTitleOfSelectedFileByCompany(string fileTitle, int fileId, int currentUserId, bool isAdmin, int currentUserCompanyId)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var companyFile = await _context.CompanyFile.FirstOrDefaultAsync(x => x.FileId == fileId && ((x.CompanyId == currentUserCompanyId) || isAdmin));
                    if (companyFile != null)
                    {
                        var file = await _context.File.FirstOrDefaultAsync(x => x.Id == companyFile.FileId);
                        if (file != null)
                        {
                            file.ActualFileTitle = fileTitle;
                            companyFile.UpdatedByUserId = currentUserId;
                            file.UpdatedByUserId = currentUserId;
                            await _context.SaveChangesAsync();
                            transaction.Commit();
                            return true;
                        }
                    }
                    throw new Exception(CoreErrorMessages.EntityNotFound);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _logger.LogError($"Error while attaching a fileId {fileId}. Error : {ex.Message}");
                }
                return false;
            }
        }

        public async Task<bool> DeleteCompanyAnnouncementAsync(int currentUserId, int announcementId, bool isAdmin)
        {
            var companyAnnouncement = await _context.CompanyAnnouncement.FirstOrDefaultAsync(x => x.Id == announcementId && (x.UserId == currentUserId || isAdmin) && x.StatusId == CompanyAnnouncementStatus.Active);
            if (companyAnnouncement != null)
            {
                companyAnnouncement.StatusId = CompanyAnnouncementStatus.Deleted;
                _context.Entry(companyAnnouncement).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                throw new CustomException(CoreErrorMessages.EntityNotFound);
            }
        }
          

        public async Task<WrapperModel<CompanyAnnouncementDTO>> GetAllCompanyAnnouncementsAsync(BaseSearchFilterModel filter, int companyId)
        {
            IQueryable<CompanyAnnouncement> companyAnnouncementQuery = _context.CompanyAnnouncement.AsNoTracking()
                                                                 .Include(i => i.Company)
                                                                 .Include(i => i.User)
                                                                 .ThenInclude(u => u.Image)
                                                                 .Include(i => i.Regions)
                                                                 .ThenInclude(i => i.Region)
                                                                 .Where(c=> c.CompanyId == companyId && c.StatusId == CompanyAnnouncementStatus.Active)
                                                                 .OrderByDescending(x => x.ModifiedOn);


            int count = 0;
            if (filter.IncludeCount)
            {
                count = await companyAnnouncementQuery.CountAsync();
                if (count == 0)
                {
                    return new WrapperModel<CompanyAnnouncementDTO>
                    {
                        DataList = new List<CompanyAnnouncementDTO>()
                    };
                }
            }

            if (filter.Skip.HasValue)
            {
                companyAnnouncementQuery = companyAnnouncementQuery.Skip(filter.Skip.Value);
            }

            if (filter.Take.HasValue)
            {
                companyAnnouncementQuery = companyAnnouncementQuery.Take(filter.Take.Value);
            }

            List<CompanyAnnouncement> companyAnnouncementDTO = await companyAnnouncementQuery.ToListAsync();

            return new WrapperModel<CompanyAnnouncementDTO>
            {
                Count = count,
                DataList = companyAnnouncementDTO.Select(_mapper.Map<CompanyAnnouncementDTO>).ToList()
            };
        }

        public async Task<bool> CreateOrUpdateCompanyAnnouncementAsync(int announcementId, CompanyAnnouncementDTO companyAnnouncementsDTO, int currentUserId, bool isAdmin, int currentUserCompanyId)
        {
            bool isEdit = announcementId > 0;
            CompanyAnnouncement? companyAnnouncement = null;

            if (isEdit)
            {
                companyAnnouncement = await _context.CompanyAnnouncement.FirstOrDefaultAsync(b => b.Id == announcementId && (isAdmin || b.CompanyId == currentUserCompanyId || b.UserId == companyAnnouncementsDTO.UserId) && b.StatusId != CompanyAnnouncementStatus.Deleted);
                if (companyAnnouncement == null)
                    throw new CustomException($"{CoreErrorMessages.ErrorOnSaving} Company Announcement does not exist.");
            }
            else
            {
                companyAnnouncement = new CompanyAnnouncement();
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (isEdit)
                    {
                        companyAnnouncementsDTO.CreatedOn = companyAnnouncement.CreatedOn;
                        companyAnnouncementsDTO.UserId = companyAnnouncement.UserId;
                        _mapper.Map(companyAnnouncementsDTO, companyAnnouncement);
                        companyAnnouncement.UpdatedByUserId = currentUserId;
                    }
                    else
                    {
                        _mapper.Map(companyAnnouncementsDTO, companyAnnouncement);
                        _context.CompanyAnnouncement.Add(companyAnnouncement);
                        await _context.SaveChangesAsync();
                    }

                    //Insert Region Data for CompanyAnnouncement
                    int noOfRegionsRowInserted = await AddCompanyAnnouncementRegionsAsync(isEdit, companyAnnouncementsDTO, companyAnnouncement.Id);


                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new CustomException(CoreErrorMessages.ErrorOnSaving, ex);
                }
            }
        }

        public async Task<CompanyAnnouncementDTO?> GetCompanyAnnouncementByIdAsync(int id)
        {            
            var companyAnnouncement = await _context.CompanyAnnouncement.Include(i => i.Regions)
                                    .ThenInclude(i => i.Region).FirstOrDefaultAsync(ca => ca.Id == id && ca.StatusId != CompanyAnnouncementStatus.Deleted);
            if(companyAnnouncement == null)
                throw new CustomException(CoreErrorMessages.EntityNotFound);
            var companyAnnouncementDTO = _mapper.Map<CompanyAnnouncementDTO>(companyAnnouncement);
            companyAnnouncementDTO.Regions = companyAnnouncement.Regions.Select(i => _mapper.Map<RegionDTO>(i.Region)).OrderBy(x => x.Name).ToList();

            return companyAnnouncementDTO;
        }

        private async Task<int> AddCompanyAnnouncementRegionsAsync(bool isEdit, CompanyAnnouncementDTO modelDTO, int announcementId)
        {
            if (isEdit)
            {
                _context.RemoveRange(_context.CompanyAnnouncementRegion.Where(ir => ir.CompanyAnnouncementId == announcementId));

            }
            _context.CompanyAnnouncementRegion.AddRange(modelDTO.RegionIds.Select(item => new CompanyAnnouncementRegion()
            {
                CompanyAnnouncementId = announcementId,
                RegionId = item
            }));

            return await _context.SaveChangesAsync();
        }
    }
}
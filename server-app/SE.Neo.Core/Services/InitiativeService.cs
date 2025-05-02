using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SE.Neo.Common.Attributes;
using SE.Neo.Common.Enums;
using SE.Neo.Common.Models;
using SE.Neo.Common.Models.CMS;
using SE.Neo.Common.Models.Company;
using SE.Neo.Common.Models.Conversation;
using SE.Neo.Common.Models.Initiative;
using SE.Neo.Common.Models.Media;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Common.Models.Tool;
using SE.Neo.Common.Models.User;
using SE.Neo.Core.Configs;
using SE.Neo.Core.Constants;
using SE.Neo.Core.Data;
using SE.Neo.Core.Entities;
using SE.Neo.Core.Entities.CMS;
using SE.Neo.Core.Enums;
using SE.Neo.Core.Models.Conversation;
using SE.Neo.Core.Services.Interfaces;
namespace SE.Neo.Core.Services
{
    /// <summary>
    /// 
    /// </summary>
    public partial class InitiativeService : BaseService, IInitiativeService
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<InitiativeService> _logger;
        private readonly IMapper _mapper;
        private readonly MemoryCacheTimeStamp _memoryCacheTimeStamp;
        private readonly ICommonService _commonService;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <param name="logger"></param>
        /// <param name="mapper"></param>
        /// <param name="commonService"></param>
        /// <param name="memoryCacheTimeStamp"></param>
        /// <param name="cache"></param>
        public InitiativeService(ApplicationContext context, ILogger<InitiativeService> logger, IMapper mapper, ICommonService commonService,
            IOptions<MemoryCacheTimeStamp> memoryCacheTimeStamp, IDistributedCache cache) : base(cache)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _memoryCacheTimeStamp = memoryCacheTimeStamp.Value;
            _commonService = commonService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <param name="initiativeDTO"></param>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        public async Task<InitiativeDTO> CreateOrUpdateInitiativeAsync(int id, InitiativeDTO initiativeDTO)
        {
            bool isEdit = id > 0;
            Initiative? initiative = null;

            if (isEdit)
            {
                initiative = await _context.Initiative.FirstOrDefaultAsync(b => b.Id == id && b.UserId == initiativeDTO.User.Id && b.StatusId != Enums.InitiativeStatus.Deleted);
                if (initiative == null)
                    throw new CustomException($"{CoreErrorMessages.ErrorOnSaving} Initiative does not exist.");
            }
            else
            {
                initiative = new Initiative();
            }

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (isEdit)
                    {
                        initiativeDTO.ProjectTypeId = initiative.ProjectTypeId;
                        initiativeDTO.CreatedOn = initiative.CreatedOn;
                        initiativeDTO.CurrentStepId = initiative.CurrentStepId;
                        _mapper.Map(initiativeDTO, initiative);
                    }
                    else
                    {
                        initiativeDTO.CurrentStepId = 1;
                        _mapper.Map(initiativeDTO, initiative);
                        _context.Initiative.Add(initiative);
                        await _context.SaveChangesAsync();
                    }

                    //Insert Region Data for Initiative
                    int noOfRegionsRowInserted = await AddInitiativeRegionsAsync(isEdit, initiativeDTO, initiative.Id);
                    int noOfCollaboratorAdded = await AddInitiativeCollaboratorAsync(isEdit, initiativeDTO, initiative.Id);

                    if (!isEdit)
                        await AddOrUpdateInitiativeRecommendationActivityRecordAsync(initiative.Id, InitiativeModule.All);

                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new CustomException(CoreErrorMessages.ErrorOnSaving, ex);
                }
            }

            return _mapper.Map(initiative, initiativeDTO);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="initiativeId"></param>
        /// <returns></returns>
        public async Task<bool> DeleteInitiativeAsync(int userId, int initiativeId)
        {
            var initiative = await _context.Initiative.FirstOrDefaultAsync(x => x.Id == initiativeId && x.UserId == userId && x.StatusId != Enums.InitiativeStatus.Deleted);
            if (initiative != null)
            {
                initiative.StatusId = Enums.InitiativeStatus.Deleted;
                _context.Entry(initiative).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                throw new CustomException(CoreErrorMessages.EntityNotFound);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <param name="roleIds"></param>
        /// <param name="initiativeId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<InitiativeContentsWrapperModel<ArticleForInitiativeDTO>> GetRecommendedArticlesForInitiativeAsync(InitiativeRecommendationRequest request, int userId, List<int> roleIds, int initiativeId)
        {
            var newlyCreatedArticleIds = new List<int>();
            var savedArticleIdsOfInitiative = new List<int>();

            IQueryable<Initiative> initiativeQueryable = _context.Initiative.Include(x => x.Regions).AsNoTracking();
            if (!request.IsCreate)
            {
                initiativeQueryable = initiativeQueryable.Include(x => x.Articles.Where(x => x.InitiativeId == initiativeId));
            }
            Initiative? initiative = await initiativeQueryable.FirstOrDefaultAsync(p => p.Id == initiativeId && p.StatusId == Enums.InitiativeStatus.Active && p.UserId == userId);


            if (initiative != null)
            {
                IQueryable<Article> articlesQueryable = _context.Articles.AsNoTracking().Where(p => (!p.IsDeleted && p.ArticleRoles.Any(r => roleIds.Contains(r.RoleId))
                && p.ArticleCategories.Any(c => c.CategoryId == initiative.ProjectTypeId)) || p.Id == request.AttachedContentId);

                if (!request.IsCreate)
                {
                    savedArticleIdsOfInitiative = initiative.Articles.Select(i => i.ArticleId).ToList();

                    if (savedArticleIdsOfInitiative.Count > 0)
                    {
                        articlesQueryable = articlesQueryable.Where(p => !savedArticleIdsOfInitiative.Any(i => i == p.Id));
                    }

                    DateTime? lastViewedDate = await GetLastViewedDateByModuleType(initiativeId, InitiativeModule.Learn);
                    newlyCreatedArticleIds = await articlesQueryable.Where(i => i.CreatedOn >= lastViewedDate).Select(i => i.Id).ToListAsync();
                }

                articlesQueryable = articlesQueryable
                       .Include(p => p.ArticleCategories.Where(s => !s.Category.IsDeleted))
                       .ThenInclude(c => c.Category);

                articlesQueryable = articlesQueryable.Include(p => p.ArticleRegions.Where(s => !s.Region.IsDeleted))
                     .ThenInclude(c => c.Region);

                articlesQueryable = articlesQueryable.Include(p => p.ArticleSolutions.Where(s => !s.Solution.IsDeleted))
                     .ThenInclude(c => c.Solution);


                articlesQueryable = articlesQueryable.Include(p => p.ArticleTechnologies.Where(s => !s.Technology.IsDeleted))
                     .ThenInclude(c => c.Technology);

                articlesQueryable = articlesQueryable.Include(p => p.ArticleContentTags.Where(s => !s.ContentTag.IsDeleted))
                     .ThenInclude(c => c.ContentTag);

                articlesQueryable = SortArticles(articlesQueryable, initiative, newlyCreatedArticleIds, request.AttachedContentId);

                int count = 0;
                int newlyCreatedArticlesCount = newlyCreatedArticleIds.Count();

                if (request.IncludeCount)
                {
                    count = await articlesQueryable.CountAsync();

                    if (count == 0)
                    {
                        return new InitiativeContentsWrapperModel<ArticleForInitiativeDTO> { Count = count, DataList = new List<ArticleForInitiativeDTO>(), NewRecommendationsCount = newlyCreatedArticlesCount };
                    }
                }

                if (request.Skip.HasValue)
                {
                    articlesQueryable = articlesQueryable.Skip(request.Skip.Value);
                }

                if (request.Take.HasValue)
                {
                    articlesQueryable = articlesQueryable.Take(request.Take.Value);
                }


                var recommendedArticles = articlesQueryable.Select(x => new ArticleForInitiativeDTO()
                {
                    Id = x.Id,
                    ImageUrl = x.ImageUrl,
                    InitiativeId = initiativeId,
                    Title = x.Title,
                    TypeId = Convert.ToInt32(x.TypeId),
                    IsNew = !request.IsCreate ? newlyCreatedArticleIds.Any(na => na == x.Id) : false,
                    Categories = x.ArticleCategories.Select(x => x.Category).Select(item => _mapper.Map<CategoryDTO>(item)),
                    TagsTotalCount = x.ArticleCategories.Count + x.ArticleRegions.Count + x.ArticleSolutions.Count + x.ArticleTechnologies.Count + x.ArticleContentTags.Count
                });

                return new InitiativeContentsWrapperModel<ArticleForInitiativeDTO> { Count = count, DataList = recommendedArticles, NewRecommendationsCount = newlyCreatedArticlesCount };
            }

            throw new Exception("Initiative is not found");
        }

        private async Task<DateTime?> AddOrUpdateInitiativeRecommendationActivityRecordAsync(int initiativeId, InitiativeModule initiativeModule)
        {
            var entity = await _context.InitiativeRecommendationActivity.FirstOrDefaultAsync(i => i.InitiativeId == initiativeId);
            var currentUTCDateTime = DateTime.UtcNow;
            DateTime? lastViewDateForModule = null;
            if (entity != null)
            {
                switch (initiativeModule)
                {
                    case InitiativeModule.Learn:
                        lastViewDateForModule = entity.ArticleLastViewedDate;
                        entity.ArticleLastViewedDate = currentUTCDateTime;
                        break;
                    case InitiativeModule.Projects:
                        lastViewDateForModule = entity.ProjectsLastViewedDate;
                        entity.ProjectsLastViewedDate = currentUTCDateTime;
                        break;
                    case InitiativeModule.Community:
                        lastViewDateForModule = entity.CommunityLastViewedDate;
                        entity.CommunityLastViewedDate = currentUTCDateTime;
                        break;
                    case InitiativeModule.Tools:
                        lastViewDateForModule = entity.ToolsLastViewedDate;
                        entity.ToolsLastViewedDate = currentUTCDateTime;
                        break;

                    case InitiativeModule.All:
                        entity.ArticleLastViewedDate = currentUTCDateTime;
                        entity.ProjectsLastViewedDate = currentUTCDateTime;
                        entity.CommunityLastViewedDate = currentUTCDateTime;
                        entity.ToolsLastViewedDate = currentUTCDateTime;
                        break;
                }
            }
            else
            {
                await _context.InitiativeRecommendationActivity.AddAsync(new InitiativeRecommendationActivity()
                {
                    InitiativeId = initiativeId,
                    ArticleLastViewedDate = currentUTCDateTime,
                    ProjectsLastViewedDate = currentUTCDateTime,
                    CommunityLastViewedDate = currentUTCDateTime,
                    ToolsLastViewedDate = currentUTCDateTime,
                });
                lastViewDateForModule = currentUTCDateTime;
            }

            await _context.SaveChangesAsync();

            return lastViewDateForModule;
        }


        /// <summary>
        /// <param name="initiativeId"></param>
        /// <param name="userId"></param>
        /// <param name="isEditMode"></param>
        /// </summary>
        /// <returns></returns>
        public async Task<InitiativeAndProgressDetailsDTO?> GetInitiativeAndProgressTrackerDetailsByIdAsync(int initiativeId, int userId, bool isEditMode, bool isAdmin)
        {
            IQueryable<Initiative> initiativeQueryable = _context.Initiative.Include(x => x.Regions).ThenInclude(x => x.Region).Include(x => x.ProjectType);
            initiativeQueryable = initiativeQueryable.Include(i => i.Collaborators.Where(c => c.User.StatusId != Enums.UserStatus.Deleted)).ThenInclude(x => x.User).Include(x => x.User);
            if (!isEditMode)
            {
                initiativeQueryable = initiativeQueryable.Include(i => i.ProgressDetails);
            }

            initiativeQueryable = initiativeQueryable.
         Where(s => s.Id == initiativeId && s.StatusId == Enums.InitiativeStatus.Active && (s.UserId == userId || isAdmin || ((s.Collaborators.Any(b => b.UserId == userId) && s.Collaborators.Any(b => b.CreatedByUserId != userId)))));
            var initiative = await initiativeQueryable.FirstOrDefaultAsync();

            InitiativeAndProgressDetailsDTO? initiativeAndProgressDetailsDTO = null;
            if (initiative != null)
            {
                if (isEditMode)
                {
                    initiativeAndProgressDetailsDTO = new InitiativeAndProgressDetailsDTO()
                    {
                        Id = initiative.Id,
                        Title = initiative.Title,
                        Category = _mapper.Map<CategoryDTO>(initiative.ProjectType),
                        Regions = initiative.Regions.Select(i => _mapper.Map<RegionDTO>(i.Region)).OrderBy(x => x.Name).ToList(),
                        ScaleId = initiative.ScaleId,
                        CurrentStepId = initiative.CurrentStepId,
                        CollaboratorIds = initiative.Collaborators.Select(c => c.UserId).ToList(),
                        Collaborators = initiative.Collaborators.Select(i => _mapper.Map<UserDTO>(i.User)).OrderBy(x => x.Username).ToList(),
                        User = _mapper.Map<UserDTO>(initiative.User)
                    };

                }
                else
                {
                    var initiativeSteps = await _context.InitiativeSteps.Include(x => x.InitiativeSubSteps.Where(y => y.CategoryId == initiative.ProjectTypeId)).ToListAsync();

                    if (initiativeSteps.Any())
                    {
                        initiativeAndProgressDetailsDTO = new InitiativeAndProgressDetailsDTO()
                        {
                            Id = initiative.Id,
                            ModifiedOn = initiative.ModifiedOn,
                            CreatedOn = initiative.CreatedOn,
                            Title = initiative.Title,
                            Category = _mapper.Map<CategoryDTO>(initiative.ProjectType),
                            Regions = initiative.Regions.Select(i => _mapper.Map<RegionDTO>(i.Region)).OrderBy(x => x.Name).ToList(),
                            CurrentStepId = initiative.CurrentStepId,
                            Steps = initiativeSteps.Select(_mapper.Map<InitiativeStepDTO>).ToList(),
                            SubStepsProgress = initiative.ProgressDetails.Select(_mapper.Map<InitiativeSubStepProgressDTO>).ToList(),
                            CollaboratorIds = initiative.Collaborators.Select(c => c.UserId).ToList(),
                            Collaborators = initiative.Collaborators.Select(i => _mapper.Map<UserDTO>(i.User)).OrderBy(x => x.Username).ToList(),
                            User = _mapper.Map<UserDTO>(initiative.User)
                        };
                    }
                }
            }
            return initiativeAndProgressDetailsDTO;

        }

        private async Task<int> AddInitiativeRegionsAsync(bool isEdit, InitiativeDTO modelDTO, int initiativeId)
        {
            if (isEdit)
            {
                _context.RemoveRange(_context.InitiativeRegions.Where(ir => ir.InitiativeId == initiativeId));

            }
            _context.InitiativeRegions.AddRange(modelDTO.RegionIds.Select(item => new InitiativeRegion()
            {
                InitiativeId = initiativeId,
                RegionId = item
            }));

            return await _context.SaveChangesAsync();

        }

        private async Task<int> AddInitiativeCollaboratorAsync(bool isEdit, InitiativeDTO modelDTO, int initiativeId)
        {
            if (isEdit)
            {
                _context.RemoveRange(_context.InitiativeCollaborator.Where(ir => ir.InitiativeId == initiativeId));

            }
            _context.InitiativeCollaborator.AddRange(modelDTO.CollaboratorIds.Select(item => new InitiativeCollaborator()
            {
                InitiativeId = initiativeId,
                UserId = item
            }));

            return await _context.SaveChangesAsync();

        }

        /// <summary>
        /// The Method is used to get initiatives by User Id. If more data is needed in the response which require more table data loading, please consider to create a separate method.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<int>> GetInitiativeIdsByUserId(int userId)
        {
            return await _context.Initiative.Where(i => i.UserId == userId && i.StatusId == Enums.InitiativeStatus.Active).Select(x => x.Id).ToListAsync();
        }

        /// <summary>
        /// Get list of saved articles of an initiative
        /// </summary>
        /// <param name="initiativeId"></param>
        /// <param name="filter"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<WrapperModel<ArticleForInitiativeDTO>> GetSavedArticlesForInitiativeAsync(int initiativeId, BaseSearchFilterModel filter, int userId, bool isAdmin)
        {
            IQueryable<InitiativeArticle> initiativeArticlesQuery = _context.InitiativeArticle.Where(i => i.InitiativeId == initiativeId && (i.Initiative.UserId == userId || isAdmin|| (_context.InitiativeCollaborator.Any(ic => ic.InitiativeId == initiativeId && ic.UserId == userId)) 
                ) && i.Initiative.StatusId == Enums.InitiativeStatus.Active)
                                                                        .Include(s => s.Article)
                                                                        .ThenInclude(s => s.ArticleCategories)
                                                                        .ThenInclude(s => s.Category)
                                                                        .Include(s => s.Article.ArticleRegions)
                                                                        .Include(s => s.Article.ArticleSolutions)
                                                                        .Include(s => s.Article.ArticleTechnologies)
                                                                        .Include(s => s.Article.ArticleContentTags).OrderByDescending(x => x.CreatedOn);

            int count = 0;
            if (filter.IncludeCount)
            {
                count = await initiativeArticlesQuery.CountAsync();
                if (count == 0)
                {
                    return new WrapperModel<ArticleForInitiativeDTO> { Count = count, DataList = new List<ArticleForInitiativeDTO>() };
                }
            }

            if (filter.Skip.HasValue)
            {
                initiativeArticlesQuery = initiativeArticlesQuery.Skip(filter.Skip.Value);
            }

            if (filter.Take.HasValue)
            {
                initiativeArticlesQuery = initiativeArticlesQuery.Take(filter.Take.Value);
            }


            var savedInitiatives = initiativeArticlesQuery.Select(x => new ArticleForInitiativeDTO()
            {
                Id = x.ArticleId,
                ImageUrl = x.Article.ImageUrl,
                InitiativeId = x.InitiativeId,
                Title = x.Article.Title,
                TypeId = Convert.ToInt32(x.Article.TypeId),
                IsNew = false,
                Categories = x.Article.ArticleCategories.Select(x => x.Category).Select(item => _mapper.Map<CategoryDTO>(item)),
                TagsTotalCount = x.Article.ArticleCategories.Count + x.Article.ArticleRegions.Count + x.Article.ArticleSolutions.Count + x.Article.ArticleTechnologies.Count + x.Article.ArticleContentTags.Count
            });

            return new WrapperModel<ArticleForInitiativeDTO>
            {
                Count = count,
                DataList = savedInitiatives
            };
        }

        /// <summary>
        /// Get list of saved files of an initiative
        /// </summary>
        /// <param name="initiativeId"></param>
        /// <param name="filter"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<WrapperModel<InitiativeFileDTO>> GetSavedFilesOfAnInitiativeAsync(int initiativeId, BaseSearchFilterModel filter, int userId, bool isAdmin)
        {
            IQueryable<InitiativeFile> initiativeFilesQuery =
                ExpandSortFiles(_context.InitiativeFile.Where(f => f.InitiativeId == initiativeId
                && (f.Initiative.UserId == userId || isAdmin || (_context.InitiativeCollaborator.Any(ic => ic.InitiativeId == initiativeId && ic.UserId == userId))
                ) && f.Initiative.StatusId == Enums.InitiativeStatus.Active), filter.Expand, filter.OrderBy);

            int count = 0;
            if (filter.IncludeCount)
            {
                count = await initiativeFilesQuery.CountAsync();
                if (count == 0)
                {
                    return new WrapperModel<InitiativeFileDTO> { Count = count, DataList = new List<InitiativeFileDTO>() };
                }
            }

            if (filter.Skip.HasValue)
            {
                initiativeFilesQuery = initiativeFilesQuery.Skip(filter.Skip.Value);
            }

            if (filter.Take.HasValue)
            {
                initiativeFilesQuery = initiativeFilesQuery.Take(filter.Take.Value);
            }


            var initiativeFiles = initiativeFilesQuery
                .Join(_context.Users, i => i.CreatedByUserId, u => u.Id, (i, u) => new { i, u })
                .Join(_context.Users, i => i.i.UpdatedByUserId, u => u.Id, (i, updatedByUser) => new { i.i, i.u, updatedByUser })
                .Select(x => new InitiativeFileDTO()
            {
                Id = x.i.FileId,
                ActualFileName = x.i.File.ActualFileName ?? string.Empty,
                ActualFileTitle = x.i.File.ActualFileTitle ?? string.Empty,
                InitiativeId = x.i.InitiativeId,
                Name = x.i.File.Name ?? string.Empty,
                Type = x.i.File.Type,
                Extension = x.i.File.Extension,
                Link = x.i.File.Link,
                Size = x.i.File.Size,
                Version = x.i.File.Version,
                CreatedOn = Convert.ToDateTime(x.i.File.CreatedOn),
                ModifiedOn = Convert.ToDateTime(x.i.File.ModifiedOn),
                CreatedByUserId =x.i.File.CreatedByUserId,
                CreatedByUser = _mapper.Map<UserDTO>(x.u),
                UpdatedByUserId = x.i.File.UpdatedByUserId,
                UpdatedByUser = _mapper.Map<UserDTO>(x.updatedByUser),
            });

            return new WrapperModel<InitiativeFileDTO>
            {
                Count = count,
                DataList = initiativeFiles
            };
        }

        /// <summary>
        /// code to save all the initiative resources
        /// </summary>
        /// <param name="initiativeContentDTO"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<bool> SaveContentsToAnInitiativeAsync(InitiativeContentDTO initiativeContentDTO, int userId)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var initiative = await _context.Initiative.FirstOrDefaultAsync(y => y.Id == initiativeContentDTO.InitiativeId && y.UserId == userId);
                    if (initiative == null)
                    {
                        throw new CustomException(CoreErrorMessages.EntityNotFound);
                    }
                    if (initiativeContentDTO.ArticleIds?.Any() == true)
                    {
                        _context.InitiativeArticle.AddRange(initiativeContentDTO.ArticleIds.Select(item => new InitiativeArticle()
                        {
                            InitiativeId = initiativeContentDTO.InitiativeId,
                            ArticleId = item
                        }));
                    }
                    if (initiativeContentDTO.ProjectIds?.Any() == true)
                    {
                        _context.InitiativeProject.AddRange(initiativeContentDTO.ProjectIds.Select(item => new InitiativeProject()
                        {
                            InitiativeId = initiativeContentDTO.InitiativeId,
                            ProjectId = item
                        }));
                    }
                    if (initiativeContentDTO.CommunityUserIds?.Any() == true)
                    {
                        _context.InitiativeCommunity.AddRange(initiativeContentDTO.CommunityUserIds.Select(item => new InitiativeCommunity()
                        {
                            InitiativeId = initiativeContentDTO.InitiativeId,
                            UserId = item
                        }));
                    }
                    if (initiativeContentDTO.ToolIds?.Any() == true)
                    {
                        _context.InitiativeTool.AddRange(initiativeContentDTO.ToolIds.Select(item => new InitiativeTool()
                        {
                            InitiativeId = initiativeContentDTO.InitiativeId,
                            ToolId = item
                        }));
                    }
                    if (initiativeContentDTO.DiscussionIds?.Any() == true)
                    {
                        _context.InitiativeConversation.AddRange(initiativeContentDTO.DiscussionIds.Select(item => new InitiativeConversation()
                        {
                            InitiativeId = initiativeContentDTO.InitiativeId,
                            DiscussionId = item
                        }));
                    }
                    if (!initiativeContentDTO.IsNew)
                    {
                        UpdateInitiativeModifiedOn(initiative);
                    }
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _logger.LogError($"Error while attaching contents to an initiative {initiativeContentDTO.InitiativeId}. Error : {ex.Message}");
                    return false;
                }

                return true;
            }
        }


        /// <summary>
        /// code to save all the initiative files
        /// </summary>
        /// <param name="initiativeFileDTO"></param>
        /// <param name="userId"></param>
        /// <param name="initiativeId"></param>
        /// <param name="isAdmin"></param>
        /// <returns></returns>
        public async Task<bool> UploadFileToAnInitiativeAsync(InitiativeFileDTO initiativeFileDTO, int userId, int initiativeId, bool isAdmin)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var initiative = await _context.Initiative.FirstOrDefaultAsync(y => y.Id == initiativeId && (isAdmin || y.UserId == userId || y.Collaborators.Any(c => c.UserId == userId)));
                    if (initiative == null)
                    {
                        throw new CustomException(CoreErrorMessages.EntityNotFound);
                    }
                    var initiativeFilesCount = await _context.InitiativeFile.Where(f => f.InitiativeId == initiativeId).CountAsync();

                    if (initiativeFilesCount >= 5)
                        throw new CustomException(CoreErrorMessages.InitiativeFileMaxLimit);

                    if (!string.IsNullOrEmpty(initiativeFileDTO.ActualFileName))
                    {
                        var file = _mapper.Map<Entities.File>(initiativeFileDTO);
                        var fileEntity = await _context.File.AddAsync(file);
                        await _context.SaveChangesAsync();

                        _context.InitiativeFile.Add(new InitiativeFile()
                        {
                            InitiativeId = initiativeId,
                            FileId = fileEntity.Entity.Id
                        });
                        await _context.SaveChangesAsync();
                        transaction.Commit();
                        return true;
                    }
                    else
                        return false;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    _logger.LogError($"Error while attaching a {initiativeFileDTO.ActualFileName} file to an initiative {initiativeFileDTO.InitiativeId}. Error : {ex.Message}");
                    return false;
                }
            }
        }


        /// <summary>
        /// The Method is used to get all initiatives 
        /// </summary>
        /// <param name="filter"></param>
        /// <returns></returns>
        public async Task<WrapperModel<InitiativeAdminDTO>> GetAllInitiativesAsync(BaseSearchFilterModel filter)
        {
            IQueryable<Initiative> initiativeQuery = _context.Initiative.AsNoTracking().Include(i => i.ProjectType)
                                                                .Include(i => i.Company)
                                                                .Include(i => i.User)
                                                                .ThenInclude(u => u.Image)
                                                                .Include(i => i.InitiativeStep)
                                                                .Include(i => i.Regions)
                                                                .ThenInclude(i => i.Region);



            if (string.IsNullOrEmpty(filter.OrderBy) && string.IsNullOrEmpty(filter.Search))// default without search text and without order by
            {
                initiativeQuery = initiativeQuery.OrderByDescending(x => x.CreatedOn);
            }

            else if (!string.IsNullOrEmpty(filter.Search) && string.IsNullOrEmpty(filter.OrderBy))//with search text but no order by
            {
                initiativeQuery = SearchAndPriorizeInitiatives(initiativeQuery, filter.Search);
            }

            else if (!string.IsNullOrEmpty(filter.OrderBy) && !string.IsNullOrEmpty(filter.Search))//with search text with order by
            {
                initiativeQuery = SearchAndPriorizeInitiatives(initiativeQuery, filter.Search);
                initiativeQuery = SortInitiatives(initiativeQuery, filter.OrderBy);
            }

            else if (string.IsNullOrEmpty(filter.Search) && !string.IsNullOrEmpty(filter.OrderBy))//without search text with order by
            {
                initiativeQuery = SortInitiatives(initiativeQuery, filter.OrderBy);//4
            }


            int count = 0;
            if (filter.IncludeCount)
            {
                count = await initiativeQuery.CountAsync();
                if (count == 0)
                {
                    return new WrapperModel<InitiativeAdminDTO>
                    {
                        DataList = new List<InitiativeAdminDTO>()
                    };
                }
            }

            if (filter.Skip.HasValue)
            {
                initiativeQuery = initiativeQuery.Skip(filter.Skip.Value);
            }

            if (filter.Take.HasValue)
            {
                initiativeQuery = initiativeQuery.Take(filter.Take.Value);
            }

            List<Initiative> initiativesAdminDTO = await initiativeQuery.ToListAsync();

            return new WrapperModel<InitiativeAdminDTO>
            {
                Count = count,
                DataList = initiativesAdminDTO.Select(_mapper.Map<InitiativeAdminDTO>).ToList()
            };

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="initiativeSubStepDTO"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        /// <exception cref="CustomException"></exception>
        public async Task<bool> UpdateInitiativeSubStepProgressAsync(InitiativeSubStepProgressDTO initiativeSubStepDTO, int userId)
        {
            var initiative = await _context.Initiative.Include(x => x.ProgressDetails.Where(y => y.SubStepId == initiativeSubStepDTO.SubStepId)).FirstOrDefaultAsync(y => y.Id == initiativeSubStepDTO.InitiativeId && y.UserId == userId);
            if (initiative != null)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        if (!initiative.ProgressDetails.Any() && initiativeSubStepDTO.IsChecked)
                        {
                            var initiativeProgressDetails = new InitiativeProgressDetails()
                            {
                                InitiativeId = initiativeSubStepDTO.InitiativeId,
                                SubStepId = initiativeSubStepDTO.SubStepId,
                                StepId = initiativeSubStepDTO.StepId,
                                IsChecked = true
                            };
                            _context.InitiativeProgressDetails.Add(initiativeProgressDetails);
                        }
                        else if (initiative.ProgressDetails.Any() && !initiativeSubStepDTO.IsChecked)
                        {
                            var initiativeProgressDetails = initiative.ProgressDetails.FirstOrDefault(y => y.SubStepId == initiativeSubStepDTO.SubStepId);
                            if (initiativeProgressDetails != null)
                            {
                                _context.InitiativeProgressDetails.Remove(initiativeProgressDetails);
                            }
                        }
                        if (initiativeSubStepDTO.CurrentStep > 0 && initiative.CurrentStepId != initiativeSubStepDTO.CurrentStep)
                        {
                            initiative.CurrentStepId = initiativeSubStepDTO.CurrentStep;
                            _context.Initiative.Update(initiative);
                        }
                        else
                        {
                            UpdateInitiativeModifiedOn(initiative);
                        }
                        await _context.SaveChangesAsync();
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
            else
            {
                throw new CustomException(CoreErrorMessages.EntityNotFound);
            }
        }



        /// <summary>
        /// The method is used to remove content from the 'learn' section of an initiative.
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="initiativeId"></param>
        /// /// <param name="contentId"></param>
        /// /// <param name="contentType"></param>
        /// <returns></returns>
        public async Task<bool> RemoveContentFromInitiativeAsync(int userId, int initiativeId, int contentId, InitiativeModule contentType, bool isAdmin)
        {
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    var initiative = await _context.Initiative.FirstOrDefaultAsync(y => y.Id == initiativeId && (isAdmin || y.UserId == userId || y.Collaborators.Any(c => c.UserId == userId)));
                    if (initiative != null)
                    {
                        switch (contentType)
                        {
                            case InitiativeModule.Files:
                                {
                                    var initiativeFile = await _context.InitiativeFile.FirstOrDefaultAsync(x => x.InitiativeId == initiativeId && x.FileId == contentId && (isAdmin || x.Initiative.UserId == userId || x.Initiative.Collaborators.Any(c => c.UserId == userId)));
                                    if (initiativeFile != null)
                                    {
                                        _context.InitiativeFile.Remove(initiativeFile);
                                        var file = await _context.File.FirstOrDefaultAsync(f => f.Id == initiativeFile.FileId);
                                        if (file != null)
                                        {
                                            _context.File.Remove(file);
                                        }
                                        else
                                        {
                                            throw new CustomException(CoreErrorMessages.EntityNotFound);
                                        }
                                    }
                                    else
                                    {
                                        throw new CustomException(CoreErrorMessages.EntityNotFound);
                                    }
                                    break;
                                }
                            case InitiativeModule.Projects:
                                {
                                    var initiativeProject = await _context.InitiativeProject.FirstOrDefaultAsync(x => x.InitiativeId == initiativeId && x.ProjectId == contentId && x.Initiative.UserId == userId);
                                    if (initiativeProject != null)
                                    {
                                        _context.InitiativeProject.Remove(initiativeProject);
                                    }
                                    else
                                    {
                                        throw new CustomException(CoreErrorMessages.EntityNotFound);
                                    }
                                    break;
                                }
                            case InitiativeModule.Learn:
                                {
                                    var initiativeArticle = await _context.InitiativeArticle.FirstOrDefaultAsync(x => x.InitiativeId == initiativeId && x.ArticleId == contentId && x.Initiative.UserId == userId);
                                    if (initiativeArticle != null)
                                    {
                                        _context.InitiativeArticle.Remove(initiativeArticle);
                                    }
                                    else
                                    {
                                        throw new CustomException(CoreErrorMessages.EntityNotFound);
                                    }
                                    break;
                                }
                            case InitiativeModule.Tools:
                                {
                                    var initiativeTool = await _context.InitiativeTool.FirstOrDefaultAsync(x => x.InitiativeId == initiativeId && x.ToolId == contentId && x.Initiative.UserId == userId);
                                    if (initiativeTool != null)
                                    {
                                        _context.InitiativeTool.Remove(initiativeTool);
                                    }
                                    else
                                    {
                                        throw new CustomException(CoreErrorMessages.EntityNotFound);
                                    }
                                    break;
                                }
                            case InitiativeModule.Community:
                                {
                                    var initiativeCommunityUser = await _context.InitiativeCommunity.FirstOrDefaultAsync(x => x.InitiativeId == initiativeId && x.UserId == contentId && x.Initiative.UserId == userId);
                                    if (initiativeCommunityUser != null)
                                    {
                                        _context.InitiativeCommunity.Remove(initiativeCommunityUser);
                                    }
                                    else
                                    {
                                        throw new CustomException(CoreErrorMessages.EntityNotFound);
                                    }
                                    break;
                                }
                            case InitiativeModule.Messages:
                                {
                                    var initiativeConversation = await _context.InitiativeConversation.FirstOrDefaultAsync(x => x.InitiativeId == initiativeId && x.DiscussionId == contentId && x.Initiative.UserId == userId);
                                    if (initiativeConversation != null)
                                    {
                                        _context.InitiativeConversation.Remove(initiativeConversation);
                                    }
                                    else
                                    {
                                        throw new CustomException(CoreErrorMessages.EntityNotFound);
                                    }
                                    break;
                                }

                            default:
                                break;
                        }
                        UpdateInitiativeModifiedOn(initiative);
                        await _context.SaveChangesAsync();
                        await transaction.CommitAsync();
                        return true;
                    }

                    return false;
                }
                catch (Exception ex)
                {
                    await transaction.RollbackAsync();
                    _logger.LogError($"Error while removing the content for the initiative id : ${initiativeId} and content id ${contentId} and content type {contentType}. Error : {ex.Message}");
                    throw;
                }
            }

        }
        /// <summary>
        /// Method to get New recommendations for an inititiative
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <param name="roleIds"></param>
        /// <returns></returns>
        public async Task<List<InitiativeRecommendationCount>> GetNewRecommendationsCountAsync(InitiativeRecommendationCountRequest request, int userId, List<int> roleIds, int userCompanyId)
        {
            List<InitiativeRecommendationCount> allRecommendationsCount = new List<InitiativeRecommendationCount>();
            List<Article>? articles = new List<Article>();
            List<Project>? projects = new List<Project>();
            List<Tool>? tools = new List<Tool>();
            List<Entities.User>? communityUsers = new List<Entities.User>();
            List<InitiativeRecommendationCount> messageUnreadCountByInitiativeIdList = new List<InitiativeRecommendationCount>();
            int articlesCount, projectsCount, toolsCount, communityUserCount = 0;
            List<InitiativeRecommendationActivity> recommendationLastViewedDateByInitiativeIds = await _context.InitiativeRecommendationActivity.Include(x => x.Initiative)
                                                                       .Where(x => request.InitiativeIds.Contains(x.Initiative.Id) && x.Initiative.StatusId == Enums.InitiativeStatus.Active
                                                                       && x.Initiative.UserId == userId)
                                                                       .Select(g => g)
                                                                       .ToListAsync();

            if (recommendationLastViewedDateByInitiativeIds.Any())
            {

                var categoryIds = recommendationLastViewedDateByInitiativeIds.Select(x => x.Initiative.ProjectTypeId).Distinct();
                var minDateFromResult = recommendationLastViewedDateByInitiativeIds.Min(x => x.ArticleLastViewedDate);
                var minDateFromProjectsResult = recommendationLastViewedDateByInitiativeIds.Min(x => x.ProjectsLastViewedDate);
                var minDateFromToolsResult = recommendationLastViewedDateByInitiativeIds.Min(x => x.ToolsLastViewedDate);
                var minDateFromUsersResult = recommendationLastViewedDateByInitiativeIds.Min(x => x.CommunityLastViewedDate);
                switch (request.InitiativeContentType)
                {
                    case InitiativeModules.Learn:
                        articles = await _context.Articles.AsNoTracking().Include(y => y.ArticleCategories).Where(p => !p.IsDeleted && p.ArticleRoles.Any(r => roleIds.Contains(r.RoleId)) &&
                            p.ArticleCategories.Any(c => categoryIds.Contains(c.CategoryId)) && p.CreatedOn > minDateFromResult)
                        .Select(x => new Article { Id = x.Id, CreatedOn = x.CreatedOn, ArticleCategories = x.ArticleCategories }).ToListAsync();
                        break;
                    case InitiativeModules.Projects:
                        projects = await _context.Projects.AsNoTracking().Where(p => (p.CreatedOn > minDateFromProjectsResult && categoryIds.Contains(p.CategoryId)) && p.StatusId == Enums.ProjectStatus.Active).
                       Select(x => new Project { Id = x.Id, CreatedOn = x.CreatedOn, CategoryId = x.CategoryId }).ToListAsync();
                        break;
                    case InitiativeModules.Tools:
                        tools = await _context.Tools.AsNoTracking().Where(t => (t.IsActive && t.Companies.Select(c => c.CompanyId).Contains(userCompanyId)
                     || t.Roles.Any(role => (role.RoleId == Convert.ToInt32(RoleType.Corporation)) || (role.RoleId == Convert.ToInt32(RoleType.All)))) && (t.CreatedOn > minDateFromToolsResult))
                        .Select(x => new Tool { Id = x.Id, CreatedOn = x.CreatedOn }).ToListAsync();
                        break;
                    case InitiativeModules.Community:
                        communityUsers = await _context.Users.AsNoTracking().Include(x => x.UserProfile).ThenInclude(c => c.Categories).Where(x => x.CreatedOn > minDateFromUsersResult && x.StatusId == Enums.UserStatus.Active).Where(x => x.Roles.Any(y => y.Role.Id ==
                          (int)RoleType.Corporation || y.Role.Id == (int)RoleType.SolutionProvider || y.Role.Id == (int)RoleType.SPAdmin)).
                           Where(x => x.UserProfile.Categories.Any(c => categoryIds.Contains(c.CategoryId))).Where(x => x.Id != userId).Select(
                           x => new Entities.User { Id = x.Id, CreatedOn = x.CreatedOn, UserProfile = x.UserProfile }).ToListAsync();
                        break;
                    case InitiativeModules.Messages:
                        messageUnreadCountByInitiativeIdList = await GetInitiativeMessageUnreadCountAsync(request.InitiativeIds, userId);
                        break;
                    case InitiativeModules.All:
                        articles = await _context.Articles.AsNoTracking().Include(y => y.ArticleCategories).Where(p => !p.IsDeleted && p.ArticleRoles.Any(r => roleIds.Contains(r.RoleId)) &&
                          p.ArticleCategories.Any(c => categoryIds.Contains(c.CategoryId)) && p.CreatedOn > minDateFromResult)
                      .Select(x => new Article { Id = x.Id, CreatedOn = x.CreatedOn, ArticleCategories = x.ArticleCategories }).ToListAsync();

                        projects = await _context.Projects.AsNoTracking().Where(p => (p.CreatedOn > minDateFromProjectsResult && categoryIds.Contains(p.CategoryId)) && p.StatusId == Enums.ProjectStatus.Active).
                      Select(x => new Project { Id = x.Id, CreatedOn = x.CreatedOn, CategoryId = x.CategoryId }).ToListAsync();

                        tools = await _context.Tools.AsNoTracking().Where(t => (t.IsActive && t.Companies.Select(c => c.CompanyId).Contains(userCompanyId)
                         || t.Roles.Any(role => (role.RoleId == Convert.ToInt32(RoleType.Corporation)) || (role.RoleId == Convert.ToInt32(RoleType.All)))) && t.CreatedOn > minDateFromToolsResult)
                        .Select(x => new Tool { Id = x.Id, CreatedOn = x.CreatedOn }).ToListAsync();

                        communityUsers = await _context.Users.AsNoTracking().Include(x => x.UserProfile).ThenInclude(c => c.Categories).Where(x => x.CreatedOn > minDateFromUsersResult && x.StatusId == Enums.UserStatus.Active).Where(x => x.Roles.Any(y => y.Role.Id ==
                          (int)RoleType.Corporation || y.Role.Id == (int)RoleType.SolutionProvider || y.Role.Id == (int)RoleType.SPAdmin)).
                           Where(x => x.UserProfile.Categories.Any(c => categoryIds.Contains(c.CategoryId))).Where(x => x.Id != userId).Select(
                           x => new Entities.User { Id = x.Id, CreatedOn = x.CreatedOn, UserProfile = x.UserProfile }).ToListAsync();

                        messageUnreadCountByInitiativeIdList = await GetInitiativeMessageUnreadCountAsync(request.InitiativeIds, userId);
                        break;

                }

                foreach (var item in recommendationLastViewedDateByInitiativeIds)
                {
                    articlesCount = articles.Where(x => x.CreatedOn > item.ArticleLastViewedDate && x.ArticleCategories.Any(c => c.CategoryId == item.Initiative.ProjectTypeId)).Count();
                    projectsCount = projects.Where(x => x.CreatedOn > item.ProjectsLastViewedDate && x.CategoryId == item.Initiative.ProjectTypeId).Count();
                    toolsCount = tools.Where(x => x.CreatedOn > item.ToolsLastViewedDate).Count();
                    communityUserCount = communityUsers.Where(y => y.CreatedOn > item.CommunityLastViewedDate && y.UserProfile.Categories.Any(x => x.CategoryId == item.Initiative.ProjectTypeId && y.Id == x.CreatedByUserId)).Count(); 
                    allRecommendationsCount.Add(new InitiativeRecommendationCount()
                    {
                        InitiativeId = item.Initiative.Id,
                        ArticlesCount = articlesCount,
                        ProjectsCount = projectsCount,
                        ToolsCount = toolsCount,
                        CommunityUsersCount = communityUserCount,
                        MessagesUnreadCount = messageUnreadCountByInitiativeIdList?.FirstOrDefault(x => x.InitiativeId == item.InitiativeId)?.MessagesUnreadCount ?? 0,
                    });
                }

            }
            return allRecommendationsCount;

        }

        private async Task<List<InitiativeRecommendationCount>> GetInitiativeMessageUnreadCountAsync(List<int> initiativeIds, int userId)
        {
            return await _context.InitiativeConversation.Include(x => x.Discussion).ThenInclude(x => x.DiscussionUsers)
             .Where(x => initiativeIds.Contains(x.InitiativeId) && x.Initiative.StatusId == Enums.InitiativeStatus.Active && x.Initiative.UserId == userId && x.Discussion.DiscussionUsers.Any(y => y.UserId == userId && y.UnreadCount > 0 && y.DiscussionId == x.DiscussionId))
             .SelectMany(x => x.Discussion.DiscussionUsers.Where(y => y.UserId == userId && y.UnreadCount > 0)
             .Select(y => new { initiativeId = x.InitiativeId, unreadCount = y.UnreadCount }))
             .GroupBy(g => g.initiativeId)
             .Select(g => new InitiativeRecommendationCount
             {
                 InitiativeId = g.Key,
                 MessagesUnreadCount = g.Sum(x => x.unreadCount)

             }).ToListAsync();

        }


        /// <summary>
        /// The method is used to get all the initiatives and its details for a user.
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<WrapperModel<InitiativeAndProgressDetailsDTO>> GetInitiativesAndProgressDetailsByUserIdAsync(int userId, InitiativeViewSource initiativeType, BaseSearchFilterModel request)
        {
            List<InitiativeAndProgressDetailsDTO> initiativeAndProgressDetailsDTOs = new List<InitiativeAndProgressDetailsDTO>();

            IQueryable<Initiative> initiativesQueryable = GetInitiativeList(initiativeType, userId);
            int count = 0;
            if (request.IncludeCount)
            {
                count = await initiativesQueryable.CountAsync();
            }

            if (request.Skip.HasValue)
            {
                initiativesQueryable = initiativesQueryable.Skip(request.Skip.Value);
            }

            if (request.Take.HasValue)
            {
                initiativesQueryable = initiativesQueryable.Take(request.Take.Value);
            }
            List<Initiative> initiatives = await initiativesQueryable.ToListAsync();
            var projectTypeIds = initiatives.Select(x => x.ProjectTypeId).Distinct();
            var initiativeSteps = await _context.InitiativeSteps.Include(x => x.InitiativeSubSteps.Where(y => projectTypeIds.Contains(y.CategoryId))).ToListAsync();
            if (initiatives?.Any() == true && initiativeSteps?.Any() == true)
            {
                foreach (Initiative initiative in initiatives)
                {
                    var initiativeStepsManipulated = initiativeSteps.Where(x => x.InitiativeSubSteps.Any(sub => sub.CategoryId == initiative.ProjectTypeId)).Select(ns => new InitiativeStepDTO
                    {
                        StepId = ns.Id,
                        Description = ns.Description,
                        Name = ns.Name,
                        SubSteps = ns.InitiativeSubSteps.Where(sub => sub.CategoryId == initiative.ProjectTypeId).Select(_mapper.Map<InitiativeSubStepDTO>)
                    });

                    InitiativeAndProgressDetailsDTO initiativeAndProgressDetailsDTO = new InitiativeAndProgressDetailsDTO()
                    {
                        Id = initiative.Id,
                        ModifiedOn = initiative.ModifiedOn,
                        CreatedOn = initiative.CreatedOn,
                        User = _mapper.Map<UserDTO>(initiative.User),
                        Title = initiative.Title,
                        Category = _mapper.Map<CategoryDTO>(initiative.ProjectType),
                        Regions = initiative.Regions.Select(i => _mapper.Map<RegionDTO>(i.Region)).ToList(),
                        CurrentStepId = initiative.CurrentStepId,
                        Steps = initiativeStepsManipulated,
                        SubStepsProgress = initiative.ProgressDetails.Select(_mapper.Map<InitiativeSubStepProgressDTO>).ToList()
                    };
                    if(initiativeType == InitiativeViewSource.YourInitiatives)
                    {
                        initiativeAndProgressDetailsDTO.Collaborators = initiative.Collaborators.Where(i => i.User.StatusId == Enums.UserStatus.Active).Select(i => _mapper.Map<UserDTO>(i.User)).ToList();
                    }
                    initiativeAndProgressDetailsDTOs.Add(initiativeAndProgressDetailsDTO);
                }
            }
            return new WrapperModel<InitiativeAndProgressDetailsDTO>
            {
                Count = count,
                DataList = initiativeAndProgressDetailsDTOs
            };
        }

        private IQueryable<Initiative> GetInitiativeList(InitiativeViewSource initiativeType, int userId)
        {

            IQueryable <Initiative> initiativesQueryable =  _context.Initiative.Include(i => i.ProgressDetails)

                                                                           .Include(i => i.Regions).ThenInclude(i => i.Region)
                                                                           .Include(i => i.ProjectType)
                                                                           .Include(i => i.User).AsNoTracking();
            switch (initiativeType)
            {
                case InitiativeViewSource.YourInitiatives:
                    {
                        initiativesQueryable = initiativesQueryable.Include(x => x.Collaborators).ThenInclude(x => x.User).ThenInclude(u => u.Image).Where(i => i.UserId == userId && i.StatusId == Enums.InitiativeStatus.Active).OrderByDescending(x => x.ModifiedOn);
                        break;
                    }
                case InitiativeViewSource.TeamsInitiatives:
                    {
                        initiativesQueryable = initiativesQueryable.Include(x => x.Collaborators).Where(i => i.StatusId == Enums.InitiativeStatus.Active && i.Collaborators.Any(c => c.UserId == userId && i.CreatedByUserId != userId)).OrderByDescending(x => x.ModifiedOn);
                        break;
                    }
                case InitiativeViewSource.Dashboard:
                    {
                        initiativesQueryable = initiativesQueryable.Include(x => x.Collaborators).Where(i => i.StatusId == Enums.InitiativeStatus.Active && (i.UserId == userId || i.Collaborators.Any(c => c.UserId == userId && i.CreatedByUserId != userId))).OrderByDescending(x => x.ModifiedOn).Take(3);
                        break;
                    }
            }

            return initiativesQueryable;
        }

        public async Task<InitiativeContentsWrapperModel<CommunityUserForInitiativeDTO>> GetRecommendedCommunityUsersForInitiativeAsync(InitiativeRecommendationRequest request, int userId, int initiativeId)
        {
            var newlyCreatedCommunityIds = new List<int>();
            var savedCommunityIdsOfInitiative = new List<int>();

            IQueryable<Initiative> initiativeQueryable = _context.Initiative.Include(x => x.Regions).AsNoTracking();
            if (!request.IsCreate)
            {
                initiativeQueryable = initiativeQueryable.Include(x => x.Users.Where(x => x.InitiativeId == initiativeId));
            }
            Initiative? initiative = await initiativeQueryable.FirstOrDefaultAsync(p => p.Id == initiativeId && p.StatusId == Enums.InitiativeStatus.Active && p.UserId == userId);

            if (initiative != null)
            {
                IQueryable<Entities.User> communityUsersQueryable = _context.Users
                .AsNoTracking()
                .Where(x => x.StatusId == Enums.UserStatus.Active)
                .Where(x => x.Roles.Any(y => y.Role.Id == (int)RoleType.Corporation || y.Role.Id == (int)RoleType.SolutionProvider || y.Role.Id == (int)RoleType.SPAdmin))
                .Where(x => x.UserProfile.Categories.Any(c => c.CategoryId == initiative.ProjectTypeId || c.UserProfile.UserId == request.AttachedContentId))
                .Where(x => x.Id != userId || x.Id == request.AttachedContentId)
                .Include(u => u.UserProfile).ThenInclude(u => u.Regions).ThenInclude(y => y.Region)
                .Include(u => u.Roles).ThenInclude(r => r.Role)
                .Include(u => u.Image)
                .Include(u => u.Company);

                if (!request.IsCreate)
                {
                    savedCommunityIdsOfInitiative = initiative.Users.Select(i => i.UserId).ToList();

                    if (savedCommunityIdsOfInitiative.Count > 0)
                    {
                        communityUsersQueryable = communityUsersQueryable.Where(p => !savedCommunityIdsOfInitiative.Any(i => i == p.Id));
                    }
                    DateTime? lastViewedDate = await GetLastViewedDateByModuleType(initiativeId, InitiativeModule.Community);
                    newlyCreatedCommunityIds = await communityUsersQueryable.Where(c => c.CreatedOn >= lastViewedDate).Select(i => i.Id).ToListAsync();
                }

                communityUsersQueryable = SortCommunityUsers(communityUsersQueryable, initiative, newlyCreatedCommunityIds, request.AttachedContentId);
                int count = 0;
                int newlyCreatedCommunityUsersCount = newlyCreatedCommunityIds.Count();
                if (request.IncludeCount)
                {
                    count = await communityUsersQueryable.CountAsync();

                    if (count == 0)
                    {
                        return new InitiativeContentsWrapperModel<CommunityUserForInitiativeDTO> { Count = count, DataList = new List<CommunityUserForInitiativeDTO>(), NewRecommendationsCount = newlyCreatedCommunityUsersCount };
                    }
                }

                if (request.Skip.HasValue)
                {
                    communityUsersQueryable = communityUsersQueryable.Skip(request.Skip.Value);
                }

                if (request.Take.HasValue)
                {
                    communityUsersQueryable = communityUsersQueryable.Take(request.Take.Value);
                }


                var recommendedCommunityUsers = communityUsersQueryable.Select(x => new CommunityUserForInitiativeDTO()
                {
                    Id = x.Id,
                    InitiativeId = initiativeId,
                    TypeId = (int)CommunityItemType.User,
                    IsNew = !request.IsCreate && newlyCreatedCommunityIds.Any(na => na == x.Id),
                    FirstName = x.FirstName,
                    LastName = x.LastName,
                    Roles = x.Roles.Where(y => y.RoleId != (int)RoleType.All).Select(x => _mapper.Map<RoleDTO>(x.Role)),
                    CompanyName = x.Company.Name,
                    ImageName = x.ImageName,
                    Image = _mapper.Map<BlobDTO>(x.Image),
                    TagsTotalCount = x.UserProfile.Categories.Count + x.UserProfile.Regions.Count,
                    Categories = x.UserProfile.Categories.Take(1).Select(x => _mapper.Map<CategoryDTO>(x.Category)),
                    JobTitle = x.UserProfile.JobTitle
                });
                return new InitiativeContentsWrapperModel<CommunityUserForInitiativeDTO> { Count = count, DataList = recommendedCommunityUsers, NewRecommendationsCount = newlyCreatedCommunityUsersCount };
            }

            throw new Exception("Initiative is not found");
        }

        /// <summary>
        /// Update the Initiative time for any activity on initiative
        /// </summary>
        /// <param name="initiative"></param>
        private void UpdateInitiativeModifiedOn(Initiative initiative)
        {
            initiative.ModifiedOn = DateTime.UtcNow;
            _context.Initiative.Update(initiative);
        }


        /// <summary>
        /// Get list of recommended messages of an initiative
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param> 
        /// <param name="initiativeId"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public async Task<WrapperModel<ConversationForInitiativeDTO>> GetRecommendedConversationsForInitiativeAsync(InitiativeRecommendationRequest request, int userId, int initiativeId)
        {

            var newlyCreatedArticleIds = new List<int>();
            var savedConversationIdsOfInitiative = new List<int>();

            IQueryable<Initiative> initiativeQueryable = _context.Initiative.AsNoTracking();
            if (!request.IsCreate)
            {
                initiativeQueryable = initiativeQueryable.Include(x => x.Conversations.Where(x => x.InitiativeId == initiativeId));
            }
            Initiative? initiative = await initiativeQueryable.FirstOrDefaultAsync(p => p.Id == initiativeId && p.StatusId == Enums.InitiativeStatus.Active && p.UserId == userId);

            if (initiative != null)
            {
                var messageSourceQuery = _context.Messages.AsNoTracking()
                    .Include(m => m.User).ThenInclude(u => u.Company)
                    .Include(c => c.Discussion).ThenInclude(c => c.DiscussionUsers).ThenInclude(x => x.User).ThenInclude(x => x.Image)
                    .Include(c => c.Discussion).ThenInclude(c => c.DiscussionUsers).ThenInclude(x => x.User).ThenInclude(x => x.Company)
                    .Include(c => c.Discussion).ThenInclude(c => c.Project)
                    .Where(m => !m.Discussion.IsDeleted && m.Discussion.Type == DiscussionType.PrivateChat && m.Discussion.DiscussionUsers.Any(c => c.UserId == userId) &&
                    (m.Discussion.Project.CategoryId == initiative.ProjectTypeId || m.Discussion.ProjectId == null || m.DiscussionId == request.AttachedContentId));

                if (!request.IsCreate)
                {
                    savedConversationIdsOfInitiative = initiative.Conversations.Select(i => i.DiscussionId).ToList();

                    if (savedConversationIdsOfInitiative.Count > 0)
                    {
                        messageSourceQuery = messageSourceQuery.Where(p => !savedConversationIdsOfInitiative.Any(i => i == p.DiscussionId));
                    }
                }

                var groupedMessageSourceQuery = messageSourceQuery.Select(m => new MessageGroupedByConversation
                {
                    Key = new MessageGroupedByConversation.ConversationKey
                    {
                        ConversationId = m.DiscussionId
                    },
                    Message = m
                });

                var formattedMessageQuery = groupedMessageSourceQuery.Select(e => e.Key).Distinct()
                    .SelectMany(key => groupedMessageSourceQuery
                        .Where(e => e.Key.ConversationId == key.ConversationId).Select(e => e.Message)
                        .OrderByDescending(m => m.CreatedOn)
                        .Take(1));

                IQueryable<Message> messageQueryBySortOrder = formattedMessageQuery.OrderByDescending(f => f.DiscussionId == request.AttachedContentId)
                    .ThenByDescending(e => e.Discussion.SourceTypeId == DiscussionSourceType.ProviderContact ? 1 : 0).ThenByDescending(x => x.Discussion.CreatedOn);

                int count = 0;
                if (request.IncludeCount)
                {
                    count = await messageQueryBySortOrder.CountAsync();
                    if (count == 0)
                    {
                        return new WrapperModel<ConversationForInitiativeDTO> { Count = count, DataList = new List<ConversationForInitiativeDTO>() };
                    }
                }

                if (request.Skip.HasValue)
                {
                    messageQueryBySortOrder = messageQueryBySortOrder.Skip(request.Skip.Value);
                }

                if (request.Take.HasValue)
                {
                    messageQueryBySortOrder = messageQueryBySortOrder.Take(request.Take.Value);
                }

                var recommendedDiscussions = await messageQueryBySortOrder.Select(x => new ConversationForInitiativeDTO
                {
                    Id = x.DiscussionId,
                    CreatedByUserId = (int)x.CreatedByUserId,
                    Subject = x.Discussion.Subject ?? (x.Discussion.Project != null ? x.Discussion.Project.Title : ""),
                    LastMessage = _mapper.Map<ConversationMessageDTO>(x),
                    Users = x.Discussion.DiscussionUsers.Take(5).Select(user => _mapper.Map<ConversationUserForInitiativeDTO>(user)).ToArray(),
                    UnreadCount = x.Discussion.DiscussionUsers.FirstOrDefault(u => u.UserId == userId).UnreadCount,
                    SourceTypeId = (int)x.Discussion.SourceTypeId,
                    UsersCount = x.Discussion.DiscussionUsers.Count()

                }).ToListAsync();

                return new WrapperModel<ConversationForInitiativeDTO> { Count = count, DataList = recommendedDiscussions };
            }

            throw new Exception("Initiative is not found");
        }


        /// <summary>
        /// Get list of saved messages of an initiative
        /// </summary>
        /// <param name="initiativeId"></param>
        /// <param name="filter"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<WrapperModel<ConversationForInitiativeDTO>> GetSavedConversationsForInitiativeAsync(int initiativeId, BaseSearchFilterModel filter, int userId, bool isAdmin)
        {

            IQueryable<InitiativeConversation> initiativeConversationQuery = _context.InitiativeConversation.AsNoTracking()
                                                                             .Where(i => i.InitiativeId == initiativeId && (i.Initiative.UserId == userId || isAdmin || (_context.InitiativeCollaborator.Any(ic => ic.InitiativeId == initiativeId && ic.UserId == userId)
                                                                             && i.Discussion.DiscussionUsers.Any(du => du.UserId == userId)))
                                                                             && i.Initiative.StatusId == Enums.InitiativeStatus.Active)
                                                                             .Include(c => c.Discussion).ThenInclude(c => c.Messages).ThenInclude(m => m.User).ThenInclude(u => u.Company)
                                                                              .Include(c => c.Discussion).ThenInclude(c => c.DiscussionUsers).ThenInclude(x => x.User).ThenInclude(x => x.Image)
                                                                               .Include(c => c.Discussion).ThenInclude(c => c.DiscussionUsers).ThenInclude(x => x.User).ThenInclude(x => x.Company)
                                                                                .Include(c => c.Discussion).ThenInclude(c => c.Project).OrderByDescending(x => x.CreatedOn);

            int count = 0;
            if (filter.IncludeCount)
            {
                count = await initiativeConversationQuery.CountAsync();
                if (count == 0)
                {
                    return new WrapperModel<ConversationForInitiativeDTO> { Count = count, DataList = new List<ConversationForInitiativeDTO>() };
                }
            }

            if (filter.Skip.HasValue)
            {
                initiativeConversationQuery = initiativeConversationQuery.Skip(filter.Skip.Value);
            }

            if (filter.Take.HasValue)
            {
                initiativeConversationQuery = initiativeConversationQuery.Take(filter.Take.Value);
            }


            var savedConversations = await initiativeConversationQuery.Select(x => new ConversationForInitiativeDTO
            {
                Id = x.DiscussionId,
                CreatedByUserId = (int)x.CreatedByUserId,
                Subject = x.Discussion.Subject ?? (x.Discussion.Project != null ? x.Discussion.Project.Title : ""),
                LastMessage = _mapper.Map<ConversationMessageDTO>(x.Discussion.Messages.OrderBy(x => x.CreatedOn).LastOrDefault()),
                Users = x.Discussion.DiscussionUsers.Take(5).Select(user => _mapper.Map<ConversationUserForInitiativeDTO>(user)).ToArray(),
                UnreadCount = isAdmin ? 0 : x.Discussion.DiscussionUsers.FirstOrDefault(u => u.UserId == userId).UnreadCount,
                SourceTypeId = (int)x.Discussion.SourceTypeId,
                UsersCount = x.Discussion.DiscussionUsers.Count()

            }).ToListAsync();

            return new WrapperModel<ConversationForInitiativeDTO>
            {
                Count = count,
                DataList = savedConversations
            };
        }

        /// <summary>
        /// Get the Recommended Tools for the Initiative
        /// </summary>
        /// <param name="request"></param>
        /// <param name="userId"></param>
        /// <param name="companyId"></param>
        /// <param name="initiativeId"></param>
        public async Task<InitiativeContentsWrapperModel<ToolForInitiativeDTO>> GetRecommendedToolsForInitiativeAsync(InitiativeRecommendationRequest request, int userId, int companyId, int initiativeId)
        {
            List<int> newlyCreatedToolIds = new();
            List<int> savedToolIdsOfInitiative = new();
            IQueryable<Tool> toolsQueryable = _context.Tools.Include(p => p.Icon).Where(t => (t.Companies.Select(c => c.CompanyId).Contains(companyId)) || t.Roles.Any(role => (role.RoleId == Convert.ToInt32(RoleType.Corporation)) || (role.RoleId == Convert.ToInt32(RoleType.All)))).OrderByDescending(t => t.Id == request.AttachedContentId);

            if (!request.IsCreate)
            {
                Initiative? initiative = await _context.Initiative.Include(x => x.Tools).FirstOrDefaultAsync(p => p.Id == initiativeId && p.StatusId == Enums.InitiativeStatus.Active && p.UserId == userId);
                savedToolIdsOfInitiative = initiative.Tools.Select(i => i.ToolId).ToList();

                if (savedToolIdsOfInitiative.Count > 0)
                {
                    toolsQueryable = toolsQueryable.Where(p => !savedToolIdsOfInitiative.Any(i => i == p.Id));
                }
                DateTime? lastViewedDate = await GetLastViewedDateByModuleType(initiativeId, InitiativeModule.Tools);
                newlyCreatedToolIds = await toolsQueryable.Where(p => p.CreatedOn >= lastViewedDate).Select(i => i.Id).ToListAsync();
                toolsQueryable = toolsQueryable.OrderByDescending(f => newlyCreatedToolIds.Any(na => na == f.Id));
            }
            int count = 0;
            int newlyCreatedToolsCount = newlyCreatedToolIds.Count();
            if (request.IncludeCount)
            {
                count = await toolsQueryable.CountAsync();
                if (count == 0)
                {
                    return new InitiativeContentsWrapperModel<ToolForInitiativeDTO> { Count = count, DataList = new List<ToolForInitiativeDTO>(), NewRecommendationsCount = newlyCreatedToolsCount };
                }
            }
            if (request.Skip.HasValue)
            {
                toolsQueryable = toolsQueryable.Skip(request.Skip.Value);
            }

            if (request.Take.HasValue)
            {
                toolsQueryable = toolsQueryable.Take(request.Take.Value);
            }
            List<ToolDTO> toolDTOs = toolsQueryable.Select(_mapper.Map<ToolDTO>).ToList();
            var recommendedTools = toolDTOs.Select(x => new ToolForInitiativeDTO()
            {
                Id = (int)x.Id,
                ImageUrl = x.Icon,
                InitiativeId = initiativeId,
                Title = x.Title,
                Description = x.Description,
                IsNew = newlyCreatedToolIds.Any(nt => nt == x.Id)
            });

            return new InitiativeContentsWrapperModel<ToolForInitiativeDTO> { Count = count, DataList = recommendedTools, NewRecommendationsCount = newlyCreatedToolsCount };
        }

        public async Task<bool> AttachContentToInitiativeAsync(int userId, AttachContentToInitiativeDTO attachContentToInitiativeDTO)
        {
            try
            {
                IQueryable<Initiative> initiatives = _context.Initiative.Where(x => attachContentToInitiativeDTO.InitiativeIds.Contains(x.Id) && x.StatusId == Enums.InitiativeStatus.Active && x.UserId == userId);
                bool contentAdded = false;
                if (initiatives.Any())
                {
                    switch (attachContentToInitiativeDTO.ContentType)
                    {
                        case InitiativeModules.Learn:

                            initiatives = initiatives.Include(x => x.Articles);
                            foreach (var initiative in initiatives)
                            {
                                bool alreadyExist = initiative.Articles.Any(x => x.InitiativeId == initiative.Id && x.ArticleId == attachContentToInitiativeDTO.ContentId);
                                if (!alreadyExist)
                                {
                                    var initiativeArticle = new InitiativeArticle()
                                    {
                                        ArticleId = attachContentToInitiativeDTO.ContentId,
                                        InitiativeId = initiative.Id,
                                    };
                                    await _context.InitiativeArticle.AddAsync(initiativeArticle);
                                    contentAdded = true;
                                }
                            }
                            break;
                        case InitiativeModules.Projects:
                            initiatives = initiatives.Include(x => x.Projects);
                            foreach (var initiative in initiatives)
                            {
                                bool alreadyExist = initiative.Projects.Any(x => x.InitiativeId == initiative.Id && x.ProjectId == attachContentToInitiativeDTO.ContentId);
                                if (!alreadyExist)
                                {
                                    var initiativeProject = new InitiativeProject()
                                    {
                                        ProjectId = attachContentToInitiativeDTO.ContentId,
                                        InitiativeId = initiative.Id,
                                    };
                                    await _context.InitiativeProject.AddAsync(initiativeProject);
                                    contentAdded = true;
                                }
                            }

                            break;
                        case InitiativeModules.Messages:
                            initiatives = initiatives.Include(x => x.Conversations);
                            foreach (var initiative in initiatives)
                            {
                                bool alreadyExist = initiative.Conversations.Any(x => x.InitiativeId == initiative.Id && x.DiscussionId == attachContentToInitiativeDTO.ContentId);
                                if (!alreadyExist)
                                {
                                    var initiativeConversation = new InitiativeConversation()
                                    {
                                        DiscussionId = attachContentToInitiativeDTO.ContentId,
                                        InitiativeId = initiative.Id,
                                    };
                                    await _context.InitiativeConversation.AddAsync(initiativeConversation);
                                    contentAdded = true;
                                }
                            }
                            break;
                        case InitiativeModules.Tools:
                            initiatives = initiatives.Include(x => x.Tools);
                            foreach (var initiative in initiatives)
                            {
                                bool alreadyExist = initiative.Tools.Any(x => x.InitiativeId == initiative.Id && x.ToolId == attachContentToInitiativeDTO.ContentId);
                                if (!alreadyExist)
                                {
                                    var initiativeTool = new InitiativeTool()
                                    {
                                        ToolId = attachContentToInitiativeDTO.ContentId,
                                        InitiativeId = initiative.Id,
                                    };
                                    await _context.InitiativeTool.AddAsync(initiativeTool);
                                    contentAdded = true;
                                }
                            }
                            break;
                        case InitiativeModules.Community:
                            initiatives = initiatives.Include(x => x.Users);
                            foreach (var initiative in initiatives)
                            {
                                bool alreadyExist = initiative.Users.Any(x => x.InitiativeId == initiative.Id && x.UserId == attachContentToInitiativeDTO.ContentId);
                                if (!alreadyExist)
                                {
                                    var initiativeCommunity = new InitiativeCommunity()
                                    {
                                        UserId = attachContentToInitiativeDTO.ContentId,
                                        InitiativeId = initiative.Id,
                                    };
                                    await _context.InitiativeCommunity.AddAsync(initiativeCommunity);
                                    contentAdded = true;
                                }
                            }
                            break;
                    }
                    if (contentAdded)
                    {
                        await _context.SaveChangesAsync();
                        return true;
                    }
                }

                return false;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while getting the initiatives for the contnent id : ${attachContentToInitiativeDTO.ContentId} and content type {attachContentToInitiativeDTO.ContentType}. Error : {ex.Message}");
                throw;
            }
        }

        public async Task<List<InitiativesAttachedContentDTO>> GetInitiativesByContentIdAsync(int userId, int contentId, InitiativeModule contentType)
        {
            try
            {
                var initiatives = _context.Initiative.Where(y => y.UserId == userId && y.StatusId == Enums.InitiativeStatus.Active);
                List<InitiativesAttachedContentDTO> initiativesAttachedContentList = new List<InitiativesAttachedContentDTO>();
                if (initiatives.Any())
                {
                    switch (contentType)
                    {
                        case InitiativeModule.Learn:
                            {
                                initiatives = initiatives.Include(x => x.Articles);
                                initiativesAttachedContentList.AddRange(from item in initiatives
                                                                        let initiativesAttachedContent = new InitiativesAttachedContentDTO()
                                                                        {
                                                                            InitiativeId = item.Id,
                                                                            IsAttached = item.Articles.Any(x => x.ArticleId == contentId),
                                                                            InitiativeName = item.Title
                                                                        }
                                                                        select initiativesAttachedContent);
                                return initiativesAttachedContentList;
                            }

                        case InitiativeModule.Projects:
                            {
                                initiatives = initiatives.Include(x => x.Projects);
                                initiativesAttachedContentList.AddRange(from item in initiatives
                                                                        let initiativesAttachedContent = new InitiativesAttachedContentDTO()
                                                                        {
                                                                            InitiativeId = item.Id,
                                                                            IsAttached = item.Projects.Any(x => x.ProjectId == contentId),
                                                                            InitiativeName = item.Title
                                                                        }
                                                                        select initiativesAttachedContent);
                                return initiativesAttachedContentList;
                            }

                        case InitiativeModule.Tools:
                            {
                                initiatives = initiatives.Include(x => x.Tools);
                                initiativesAttachedContentList.AddRange(from item in initiatives
                                                                        let initiativesAttachedContent = new InitiativesAttachedContentDTO()
                                                                        {
                                                                            InitiativeId = item.Id,
                                                                            IsAttached = item.Tools.Any(x => x.ToolId == contentId),
                                                                            InitiativeName = item.Title
                                                                        }
                                                                        select initiativesAttachedContent);
                                return initiativesAttachedContentList;
                            }


                        case InitiativeModule.Messages:
                            {
                                initiatives = initiatives.Include(x => x.Conversations);
                                initiativesAttachedContentList.AddRange(from item in initiatives
                                                                        let initiativesAttachedContent = new InitiativesAttachedContentDTO()
                                                                        {
                                                                            InitiativeId = item.Id,
                                                                            IsAttached = item.Conversations.Any(x => x.DiscussionId == contentId),
                                                                            InitiativeName = item.Title
                                                                        }
                                                                        select initiativesAttachedContent);
                                return initiativesAttachedContentList;
                            }
                        case InitiativeModule.Community:
                            {
                                initiatives = initiatives.Include(x => x.Users);
                                initiativesAttachedContentList.AddRange(from item in initiatives
                                                                        let initiativesAttachedContent = new InitiativesAttachedContentDTO()
                                                                        {
                                                                            InitiativeId = item.Id,
                                                                            IsAttached = item.Users.Any(x => x.UserId == contentId),
                                                                            InitiativeName = item.Title
                                                                        }
                                                                        select initiativesAttachedContent);
                                return initiativesAttachedContentList;
                            }
                    }
                }

                return initiativesAttachedContentList;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error while getting the initiatives for the contnent id : ${contentId} and content type {contentType}. Error : {ex.Message}");
                throw;
            }
        }

        public async Task<WrapperModel<ToolForInitiativeDTO>> GetSavedToolsForInitiativeAsync(int initiativeId, BaseSearchFilterModel filter, int userId, bool isAdmin)
        {
            IQueryable<InitiativeTool> initiativeToolQuery = _context.InitiativeTool.Where(i => i.InitiativeId == initiativeId && (i.Initiative.UserId == userId || isAdmin || (_context.InitiativeCollaborator.Any(ic => ic.InitiativeId == initiativeId && ic.UserId == userId))
                ) && i.Initiative.StatusId == Enums.InitiativeStatus.Active && i.Tool.IsActive == true)
                                                            .Include(x => x.Tool)
                                                            .Include(x => x.Tool.Icon)
                                                            .OrderByDescending(x => x.CreatedOn);

            int count = 0;
            if (filter.IncludeCount)
            {
                count = await initiativeToolQuery.CountAsync();
                if (count == 0)
                {
                    return new WrapperModel<ToolForInitiativeDTO> { Count = count, DataList = new List<ToolForInitiativeDTO>() };
                }
            }

            if (filter.Skip.HasValue)
            {
                initiativeToolQuery = initiativeToolQuery.Skip(filter.Skip.Value);
            }

            if (filter.Take.HasValue)
            {
                initiativeToolQuery = initiativeToolQuery.Take(filter.Take.Value);
            }


            var savedTools = await initiativeToolQuery.Select(item => _mapper.Map<ToolForInitiativeDTO>(item)).ToListAsync();

            return new WrapperModel<ToolForInitiativeDTO>
            {
                Count = count,
                DataList = savedTools
            };

        }
        public async Task<InitiativeContentsWrapperModel<ProjectForInitiativeDTO>> GetRecommendedProjectsForInitiativeAsync(InitiativeRecommendationRequest request, int userId, List<int> roleIds, int initiativeId)
        {
            var newlyCreatedProjectIds = new List<int>();
            IQueryable<Initiative> initiativeQueryable = _context.Initiative.Include(x => x.Regions).AsNoTracking();
            if (!request.IsCreate)
            {
                initiativeQueryable = initiativeQueryable.Include(x => x.Projects.Where(x => x.InitiativeId == initiativeId));
            }
            Initiative? initiative = await initiativeQueryable.FirstOrDefaultAsync(p => p.Id == initiativeId && p.StatusId == Enums.InitiativeStatus.Active && p.UserId == userId);


            if (initiative != null)
            {
                IQueryable<Project> projectsQueryable = _context.Projects.AsNoTracking().Where(p => (p.CategoryId == initiative.ProjectTypeId || p.Id == request.AttachedContentId) && p.StatusId == Enums.ProjectStatus.Active);


                if (!request.IsCreate)
                {
                    var savedProjectsIdsOfInitiative = initiative.Projects.Select(i => i.ProjectId).ToList();
                    if (savedProjectsIdsOfInitiative.Count > 0)
                    {
                        projectsQueryable = projectsQueryable.Where(p => !savedProjectsIdsOfInitiative.Any(i => i == p.Id));
                    }
                    DateTime? lastViewedDate = await GetLastViewedDateByModuleType(initiativeId, InitiativeModule.Projects);
                    newlyCreatedProjectIds = await projectsQueryable.Where(p => p.CreatedOn >= lastViewedDate).Select(i => i.Id).ToListAsync();
                }
                projectsQueryable = projectsQueryable
                      .Include(c => c.Category).Where(p => !p.Category.IsDeleted);


                projectsQueryable = projectsQueryable.Include(p => p.Regions.Where(s => !s.Region.IsDeleted))
                     .ThenInclude(c => c.Region);

                projectsQueryable = projectsQueryable.Include(p => p.Company).ThenInclude(c => c.Image);
                projectsQueryable = SortProjects(projectsQueryable, initiative, newlyCreatedProjectIds, request.AttachedContentId);

                int count = 0;
                int newlyCreatedProjectsCount = newlyCreatedProjectIds.Count();
                if (request.IncludeCount)
                {
                    count = await projectsQueryable.CountAsync();

                    if (count == 0)
                    {
                        return new InitiativeContentsWrapperModel<ProjectForInitiativeDTO> { Count = count, DataList = new List<ProjectForInitiativeDTO>(), NewRecommendationsCount = newlyCreatedProjectsCount };
                    }
                }

                if (request.Skip.HasValue)
                {
                    projectsQueryable = projectsQueryable.Skip(request.Skip.Value);
                }

                if (request.Take.HasValue)
                {
                    projectsQueryable = projectsQueryable.Take(request.Take.Value);
                }


                var recommendedProjects = projectsQueryable.Select(x => new ProjectForInitiativeDTO()
                {
                    Id = x.Id,
                    Title = x.Title,
                    SubTitle = x.SubTitle,
                    Company = _mapper.Map<CompanyDTO>(x.Company),
                    Category = _mapper.Map<CategoryDTO>(x.Category),
                    Regions = x.Regions.Select(x => x.Region).Select(x => _mapper.Map<RegionDTO>(x)),
                    IsNew = !request.IsCreate ? newlyCreatedProjectIds.Any(np => np == x.Id) : false,
                });

                return new InitiativeContentsWrapperModel<ProjectForInitiativeDTO> { Count = count, DataList = recommendedProjects, NewRecommendationsCount = newlyCreatedProjectsCount };
            }

            throw new Exception("Initiative is not found");
        }

        public async Task<(FileExistResponseDTO?, int)> ValidateFileCountAndIfExistsByInitiativeIdAsync(int initiativeId, string fileName, int userId, bool isAdmin)
        {
            var initiative = await _context.Initiative.FirstOrDefaultAsync(b => b.Id == initiativeId && b.StatusId != Enums.InitiativeStatus.Deleted && (isAdmin || b.UserId == userId || b.Collaborators.Any(c => c.UserId == userId)));
            if (initiative == null)
                throw new CustomException($"{CoreErrorMessages.ErrorOnSaving} Initiative does not exist.");

            var initiativeFilesCount = await _context.InitiativeFile.Where(f => f.InitiativeId == initiativeId).CountAsync();

            if (initiativeFilesCount >= 5)
                return (null, initiativeFilesCount);

            var fileActualName = fileName.Substring(0, fileName.LastIndexOf("."));
            var fileExtension = (FileExtension)Enum.Parse(typeof(FileExtension), fileName.Substring(fileName.LastIndexOf(".") + 1).ToLower());

            // Here the order by file id descening applied in order to get the latest document uploaded with same name 
            var fileDetails = await _context.File.Join(_context.InitiativeFile, file => file.Id, inf => inf.FileId, (file, inf) => new { File = file, InitiativeFile = inf })
                .Where(f => f.File.ActualFileTitle != null && f.File.ActualFileTitle == fileActualName &&
                    f.File.Extension == fileExtension && f.InitiativeFile.InitiativeId == initiativeId)
                .OrderByDescending(f => f.File.Version).FirstOrDefaultAsync();

            var fileExistResponse = new FileExistResponseDTO()
            {
                IsExist = fileDetails != null,
                BlobName = fileDetails?.File?.Name ?? string.Empty,
                ActualFileName = fileDetails?.File?.ActualFileName ?? string.Empty,
                ActualFileTitle = fileDetails?.File?.ActualFileTitle ?? string.Empty,
                FileVersion = fileDetails?.File?.Version ?? 0,
                IsOwner = fileDetails?.File?.UpdatedByUserId == userId || initiative.UserId == userId || isAdmin,
            };
            return (fileExistResponse, initiativeFilesCount);
        }

        public async Task<string> GetBlobFileName(int fileId)
        {
            var fileDetails = await _context.File.FirstOrDefaultAsync(f => f.Id == fileId);
            return fileDetails?.Name ?? string.Empty;
        }

        public async Task<WrapperModel<ProjectForInitiativeDTO>> GetSavedProjectsForInitiativeAsync(int initiativeId, BaseSearchFilterModel filter, int userId, bool isAdmin)
        {
            IQueryable<InitiativeProject> initiativeProjectQuery = _context.InitiativeProject.Where(i => i.InitiativeId == initiativeId && (i.Initiative.UserId == userId || isAdmin || (_context.InitiativeCollaborator.Any(ic => ic.InitiativeId == initiativeId && ic.UserId == userId))
                ) && i.Initiative.StatusId == Enums.InitiativeStatus.Active && i.Project.StatusId == Enums.ProjectStatus.Active)
                                                            .Include(x => x.Project)
                                                            .Include(x => x.Project.Company).ThenInclude(x => x.Image)
                                                            .Include(x => x.Project.Regions.Where(s => !s.Region.IsDeleted)).ThenInclude(c => c.Region)
                                                            .Include(x => x.Project.Category).Where(p => !p.Project.Category.IsDeleted)
                                                            .OrderByDescending(x => x.CreatedOn);

            int count = 0;
            if (filter.IncludeCount)
            {
                count = await initiativeProjectQuery.CountAsync();
                if (count == 0)
                {
                    return new WrapperModel<ProjectForInitiativeDTO> { Count = count, DataList = new List<ProjectForInitiativeDTO>() };
                }
            }

            if (filter.Skip.HasValue)
            {
                initiativeProjectQuery = initiativeProjectQuery.Skip(filter.Skip.Value);
            }

            if (filter.Take.HasValue)
            {
                initiativeProjectQuery = initiativeProjectQuery.Take(filter.Take.Value);
            }

            var savedProjects = await initiativeProjectQuery.Select(item => _mapper.Map<ProjectForInitiativeDTO>(item)).ToListAsync();

            return new WrapperModel<ProjectForInitiativeDTO>
            {
                Count = count,
                DataList = savedProjects
            };
        }


        public async Task<WrapperModel<CommunityUserForInitiativeDTO>> GetSavedCommunityUsersForInitiativeAsync(int initiativeId, BaseSearchFilterModel filter, int userId, bool isAdmin)
        {
            IQueryable<InitiativeCommunity> initiativeCommunityUsersQuery = _context.InitiativeCommunity.Where(i => i.InitiativeId == initiativeId && (i.Initiative.UserId == userId || isAdmin || (_context.InitiativeCollaborator.Any(ic => ic.InitiativeId == initiativeId && ic.UserId == userId))
                ) && i.Initiative.StatusId == Enums.InitiativeStatus.Active && i.User.StatusId == Enums.UserStatus.Active)
                                                            .Include(x => x.User)
                                                            .Include(x => x.User.UserProfile)
                                                            .Include(x => x.User.Image)
                                                            .Include(x => x.User.Company)
                                                            .Include(x => x.User.Roles).ThenInclude(y => y.Role)
                                                            .Include(x => x.User.UserProfile.Regions.Where(s => !s.Region.IsDeleted)).ThenInclude(c => c.Region)
                                                            .Include(x => x.User.UserProfile.Categories.Where(p => !p.Category.IsDeleted)).ThenInclude(c => c.Category)
                                                            .OrderByDescending(x => x.CreatedOn);

            int count = 0;
            if (filter.IncludeCount)
            {
                count = await initiativeCommunityUsersQuery.CountAsync();
                if (count == 0)
                {
                    return new WrapperModel<CommunityUserForInitiativeDTO> { Count = count, DataList = new List<CommunityUserForInitiativeDTO>() };
                }
            }

            if (filter.Skip.HasValue)
            {
                initiativeCommunityUsersQuery = initiativeCommunityUsersQuery.Skip(filter.Skip.Value);
            }

            if (filter.Take.HasValue)
            {
                initiativeCommunityUsersQuery = initiativeCommunityUsersQuery.Take(filter.Take.Value);
            }

            var savedUsers = await initiativeCommunityUsersQuery.Select(item => _mapper.Map<CommunityUserForInitiativeDTO>(item)).ToListAsync();

            return new WrapperModel<CommunityUserForInitiativeDTO>
            {
                Count = count,
                DataList = savedUsers
            };
        }
        public async Task<bool> UpdateInitiativeContentLastViewedDate(InitiativeContentRecommendationActivityRequest request)
        {
            switch (request.ContentType)
            {
                case InitiativeModules.Learn:
                    var result = await AddOrUpdateInitiativeRecommendationActivityRecordAsync(request.InitiativeId, InitiativeModule.Learn);
                    return result == null ? false : true;
                case InitiativeModules.Projects:
                    var projectsResult = await AddOrUpdateInitiativeRecommendationActivityRecordAsync(request.InitiativeId, InitiativeModule.Projects);
                    return projectsResult == null ? false : true;
                case InitiativeModules.Community:
                    var communityResult = await AddOrUpdateInitiativeRecommendationActivityRecordAsync(request.InitiativeId, InitiativeModule.Community);
                    return communityResult == null ? false : true;
                case InitiativeModules.Tools:
                    var toolsResult = await AddOrUpdateInitiativeRecommendationActivityRecordAsync(request.InitiativeId, InitiativeModule.Tools);
                    return toolsResult == null ? false : true;

            }
            return false;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="initiativeId"></param>
        /// <param name="initiativeModule"></param>
        /// <returns></returns>
        public async Task<DateTime?> GetLastViewedDateByModuleType(int initiativeId, InitiativeModule initiativeModule)
        {
            DateTime? moduleLastViewedDate = DateTime.UtcNow;
            var entity = await _context.InitiativeRecommendationActivity.FirstOrDefaultAsync(i => i.InitiativeId == initiativeId);
            if (entity != null)
            {
                switch (initiativeModule)
                {
                    case InitiativeModule.Learn:
                        return entity?.ArticleLastViewedDate;
                    case InitiativeModule.Projects:
                        return entity?.ProjectsLastViewedDate;
                    case InitiativeModule.Tools:
                        return entity?.ToolsLastViewedDate;
                    case InitiativeModule.Community:
                        return entity?.CommunityLastViewedDate;
                }
            }
            return moduleLastViewedDate;
        }

        /// <summary>
        /// Update the file modified date
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="fileSize"></param>
        /// <param name="initiativeId"></param>
        /// <param name="currentUserId"></param>
        /// <returns></returns>
        public async Task<bool> UpdateInitiativeFileModifiedDateAndSize(string fileName, int fileSize, int initiativeId, int currentUserId)
        {
            var file = await _context.File.FirstOrDefaultAsync(x => x.Name == fileName);
            var isCollaborator = _context.Initiative.Any(x => x.Id == initiativeId && x.UserId != currentUserId && x.Collaborators.Any(y => y.UserId == currentUserId));
            var initiativeFile = await _context.InitiativeFile.FirstOrDefaultAsync(x => x.FileId == file.Id && x.InitiativeId == initiativeId);
            if (file != null && initiativeFile != null)
            {
                if (!isCollaborator || (isCollaborator && file?.CreatedByUserId == currentUserId))
                {
                _context.Entry(file).Entity.ModifiedOn = DateTime.UtcNow;
                _context.Entry(file).Entity.Size = fileSize;
                _context.Entry(file).Entity.UpdatedByUserId = currentUserId;
                _context.Entry(initiativeFile).Entity.UpdatedByUserId = currentUserId;
                await _context.SaveChangesAsync();
                return true;
                }
                else
                {

                throw new CustomException(CoreErrorMessages.InitiativeReplaceNoAccess);
                
                }
            }
            else
            {
                throw new CustomException(CoreErrorMessages.EntityNotFound);
            }
        }
    }
}

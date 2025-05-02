using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using SE.Neo.Common.Attributes;
using SE.Neo.Common.Enums;
using SE.Neo.Common.Models.CMS;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Common.Models.User;
using SE.Neo.Common.Models.UserProfile;
using SE.Neo.Core.Constants;
using SE.Neo.Core.Data;
using SE.Neo.Core.Entities;
using SE.Neo.Core.Services.Interfaces;
using System.Data;

namespace SE.Neo.Core.Services
{
    public partial class UserProfileService : BaseService, IUserProfileService
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<UserProfileService> _logger;
        private readonly IMapper _mapper;

        public UserProfileService(ApplicationContext context, ILogger<UserProfileService> logger, IMapper mapper, IDistributedCache cache) : base(cache)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<WrapperModel<UserProfileDTO>> GetUserProfilesAsync(BaseSearchFilterModel filter, int userId)
        {
            var userProfilesQueryable = ExpandUserProfiles(_context.UserProfiles.AsNoTracking(), filter.Expand, filter.OrderBy);

            userProfilesQueryable = FilterSearchUserProfiles(userProfilesQueryable, filter.Search, filter.FilterBy);

            int count = 0;
            if (filter.IncludeCount)
            {
                count = await userProfilesQueryable.CountAsync();
                if (count == 0)
                {
                    return new WrapperModel<UserProfileDTO> { Count = count, DataList = new List<UserProfileDTO>() };
                }
            }

            if (filter.Skip.HasValue)
            {
                userProfilesQueryable = userProfilesQueryable.Skip(filter.Skip.Value);
            }

            if (filter.Take.HasValue)
            {
                userProfilesQueryable = userProfilesQueryable.Take(filter.Take.Value);
            }

            IEnumerable<UserProfile> userProfiles = await userProfilesQueryable.ToListAsync();
            List<UserProfileDTO> userProfileDTOs = userProfiles.Select(_mapper.Map<UserProfileDTO>).ToList();

            if (!string.IsNullOrEmpty(filter.Expand))
            {
                string expand = filter.Expand.ToLower();
                if (expand.Contains("followers"))
                {
                    List<UserFollower> userProfileFollowers =
                        await _context.UserFollowers.Where(f => userProfileDTOs.Select(cdto => cdto.UserId).Contains(f.FollowedId)).AsNoTracking().ToListAsync();
                    for (int i = 0; i < userProfileDTOs.Count; i++)
                    {
                        IEnumerable<UserFollower> followers = userProfileFollowers.Where(f => f.FollowedId.Equals(userProfileDTOs[i].UserId));
                        userProfileDTOs[i].FollowersCount = followers.Count();
                        userProfileDTOs[i].IsFollowed = followers.Any(df => df.FollowerId == userId);
                    }
                }
            }
            return new WrapperModel<UserProfileDTO> { Count = count, DataList = userProfileDTOs };
        }

        public async Task<UserProfileDTO?> GetUserProfileAsync(int id, int userId, string? expand)
        {
            expand = (expand == null || expand.Contains("user")) ? expand : (expand += "user");
            var usersQueryable = ExpandUserProfiles(_context.UserProfiles.AsNoTracking(), expand);
            var user = await usersQueryable.Where(s => !s.User.StatusId.Equals(Enums.UserStatus.Deleted) && s.UserId == id).FirstOrDefaultAsync();
            var userProfileDTO = _mapper.Map<UserProfileDTO>(user);

            if (!string.IsNullOrEmpty(expand) && userProfileDTO != null)
            {
                expand = expand.ToLower();
                if (expand.Contains("followers"))
                {
                    IEnumerable<UserFollower> followed =
                       await _context.UserFollowers.Where(f => f.FollowerId.Equals(userId)).AsNoTracking().ToListAsync();

                    IEnumerable<UserFollower> followers =
                        await _context.UserFollowers.Include(c => c.Follower).ThenInclude(c => c.Company).Include(c => c.Follower).ThenInclude(c => c.Image)
                        .Include(c => c.Follower).ThenInclude(c => c.UserProfile).Where(f => f.FollowedId.Equals(id)).AsNoTracking().OrderBy(o => o.Follower.FirstName)
                        .ThenBy(o => o.Follower.LastName).ToListAsync();

                    userProfileDTO.FollowersCount = followers.Count();
                    userProfileDTO.Followers = followers.Select(_mapper.Map<UserFollowerDTO>).ToList();
                    foreach (UserFollowerDTO follower in userProfileDTO.Followers)
                    {
                        follower.isFollowed = followed.Any(df => df.FollowerId == userId && df.FollowedId == follower.FollowerId);
                    }
                    userProfileDTO.IsFollowed = followers.Any(df => df.FollowerId == userId);
                }
            }
            return userProfileDTO;
        }

        public async Task<int> CreateUpdateUserProfileAsync(int id, UserProfileDTO modelDTO, bool? isEditCurrentProfile = false)
        {
            bool isEdit = id > 0;
            var model = new UserProfile();
            User user = new User();
            if (isEdit)
            {
                model = _context.UserProfiles.Include(z => z.User).ThenInclude(y => y.Roles).SingleOrDefault(b => b.UserId == id);
                if (model == null)
                    throw new CustomException($"{CoreErrorMessages.ErrorOnSaving} User Profile does not exist.");
            }

            int userId = isEdit ? id : 0;
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _mapper.Map(modelDTO, model);
                    if (!isEdit)
                    {
                        _context.UserProfiles.AddRange(model);
                        var userModel = _context.Users.Include(y => y.Roles).SingleOrDefault(b => b.Id == model.UserId);

                        // Update user status to Active when creating UserProfile.
                        if (userModel != null) userModel.StatusId = Enums.UserStatus.Active;
                        CompanyDomain? companyDomain = _context.CompanyDomains.SingleOrDefault(cd => cd.CompanyId == userModel.CompanyId && userModel.Email.Contains(cd.DomainName) && !cd.IsActive);
                        if (companyDomain != null)
                            companyDomain.IsActive = true;

                        _context.SaveChanges();
                        userId = model.UserId;

                        _cache.Remove(userModel.Username);

                        _logger.LogInformation(String.Format("User profile is creating... {0} is removed from cache.", userModel.Username));
                    }
                    //    Remove CMS dependencies
                    if (modelDTO.Categories.Any())
                        _context.RemoveRange(_context.UserProfileCategories.Where(a => a.UserProfileId == userId));

                    if (modelDTO.Regions.Any())
                        _context.RemoveRange(_context.UserProfileRegions.Where(a => a.UserProfileId == userId));

                    //    Add new CMS dependencies
                    _context.UserProfileCategories.AddRange(modelDTO.Categories.Select(item => new UserProfileCategory() { UserProfileId = userId, CategoryId = item.Id }));
                    _context.UserProfileRegions.AddRange(modelDTO.Regions.Select(item => new UserProfileRegion() { UserProfileId = userId, RegionId = item.Id }));

                    //Handle Url Links
                    _context.RemoveRange(_context.UserProfileUrlLinks.Where(a => a.UserProfileId == userId));
                    _context.UserProfileUrlLinks.AddRange(modelDTO.UrlLinks.Select(item => new UserProfileUrlLink() { UserProfileId = userId, UrlLink = item.UrlLink, UrlName = item.UrlName }));
                    if (model.User.Roles.Any(x => x.RoleId == (int)RoleType.Corporation) || model.User.Roles.Any(x => x.RoleId == (int)RoleType.SPAdmin) || model.User.Roles.Any(x => x.RoleId == (int)RoleType.SolutionProvider))
                    {
                        if (modelDTO.SkillsByCategory != null)
                        {
                            _context.RemoveRange(_context.UserSkillsByCategory.Where(sc => sc.UserProfileId == userId));
                        }
                        _context.UserSkillsByCategory.AddRange(modelDTO.SkillsByCategory.Select(item => new UserSkillsByCategory() { UserProfileId = userId, CategoryId = item.CategoryId, SkillId = item.SkillId }));
                    }

                    await _context.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new CustomException(CoreErrorMessages.ErrorOnSaving, ex);
                }
            }
            return userId;
        }

        public async Task CreateUserProfileInterestAsync(int userId, TaxonomyDTO modelDTO)
        {
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    //    Remove CMS dependencies
                    if (modelDTO.categories.Any())
                        _context.RemoveRange(_context.UserProfileCategories.Where(a => a.UserProfileId == userId));

                    if (modelDTO.regions.Any())
                        _context.RemoveRange(_context.UserProfileRegions.Where(a => a.UserProfileId == userId));

                    //    Add new CMS dependencies
                    _context.UserProfileCategories.AddRange(modelDTO.categories.Select(item => new UserProfileCategory() { UserProfileId = userId, CategoryId = item.Id }));
                    _context.UserProfileRegions.AddRange(modelDTO.regions.Select(item => new UserProfileRegion() { UserProfileId = userId, RegionId = item.Id }));

                    await _context.SaveChangesAsync();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new CustomException(CoreErrorMessages.ErrorOnSaving, ex);
                }
            }
        }

        public async Task<UserProfileSuggestionDTO> GetUserProfileSuggestionsAsync(int userId)
        {
            UserProfile? user = await _context.UserProfiles.AsNoTracking().FirstOrDefaultAsync(p => p.UserId == userId);

            List<SuggestionDTO> SuggestionDTO = new List<SuggestionDTO>();
            if (user != null)
            {
                if (string.IsNullOrWhiteSpace(user.About))
                    SuggestionDTO.Add(new SuggestionDTO() { Id = Guid.NewGuid(), Type = SuggestionType.MissingFieldData, Name = "About" });
                if (string.IsNullOrWhiteSpace(user.LinkedInUrl))
                    SuggestionDTO.Add(new SuggestionDTO() { Id = Guid.NewGuid(), Type = SuggestionType.MissingFieldData, Name = "LinkedIn" });
            }
            return new UserProfileSuggestionDTO() { Suggestions = SuggestionDTO };
        }




        public async Task<int> SyncUserLoginCountAsync()
        {
            try
            {
                SqlParameter[] param = GetInputAndOutputParams();
                int updatedRowsCount = await _context.Database.ExecuteSqlRawAsync("[dbo].[UpdateUserLoginCount] @CreatedOn, @UpdatedUserIds OUTPUT", param);
                if (param[1].Value != DBNull.Value)
                    _logger.LogInformation($"{(string)param[1].Value} users login count synced successfully");
                return updatedRowsCount;
            }
            catch (Exception ex)
            {
                _logger.LogError(CoreErrorMessages.ErrorOnSaving);
                throw new CustomException(CoreErrorMessages.ErrorOnSaving, ex);
            }

        }


        private SqlParameter[] GetInputAndOutputParams()
        {

            return new SqlParameter[] {
                         new SqlParameter() {
                            ParameterName = "@CreatedOn",
                            SqlDbType =  System.Data.SqlDbType.DateTime2,
                            Direction = System.Data.ParameterDirection.Input,
                            Value = DateTime.UtcNow.AddDays(-1)
                         },
                         new SqlParameter(){
                          ParameterName = "@UpdatedUserIds",
                          SqlDbType =  System.Data.SqlDbType.NVarChar,
                          Size = 4000,
                          Direction = ParameterDirection.Output
                         }

            };
        }

    }
}
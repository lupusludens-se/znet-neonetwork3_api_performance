using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SE.Neo.Common.Attributes;
using SE.Neo.Common.Enums;
using SE.Neo.Common.Models.EmailAlert;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Common.Models.User;
using SE.Neo.Common.Models.UserProfile;
using SE.Neo.Core.Configs;
using SE.Neo.Core.Constants;
using SE.Neo.Core.Data;
using SE.Neo.Core.Entities;
using SE.Neo.Core.Enums;
using SE.Neo.Core.Services.Interfaces;
using System.Net.Mail;
using UserStatus = SE.Neo.Core.Enums.UserStatus;

namespace SE.Neo.Core.Services
{
    public partial class UserService : BaseService, IUserService
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<UserService> _logger;
        private readonly IMapper _mapper;
        private readonly MemoryCacheTimeStamp _memoryCacheTimeStamp;

        public UserService(ApplicationContext context, ILogger<UserService> logger, IMapper mapper,
                IOptions<MemoryCacheTimeStamp> memoryCacheTimeStamp, IDistributedCache cache) : base(cache)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
            _memoryCacheTimeStamp = memoryCacheTimeStamp.Value;
        }

        #region User Management

        public async Task<WrapperModel<UserDTO>> GetUsersAsync(BaseSearchFilterModel filter, int userId, IEnumerable<int> userRoleIds, int companyId, bool isOwnCompanyUsersRequest = false, bool accessPrivateInfo = false)
        {

            var usersQueryable = ExpandSortUsers(_context.Users.AsNoTracking(), filter.Expand, filter.OrderBy, accessPrivateInfo);

            usersQueryable = FilterSearchUsers(usersQueryable, filter.Search, filter.FilterBy, accessPrivateInfo);

            usersQueryable = usersQueryable.Where(s => !s.StatusId.Equals(Enums.UserStatus.Deleted));

            if (userRoleIds.Any(r => r.Equals((int)RoleType.SPAdmin) || r.Equals((int)RoleType.Corporation)) && isOwnCompanyUsersRequest)
            {
                usersQueryable = usersQueryable.Where(s => s.StatusId != Enums.UserStatus.Inactive && s.StatusId != Enums.UserStatus.Deleted && s.CompanyId == companyId);
            }

            int count = 0;
            if (filter.IncludeCount)
            {
                count = string.IsNullOrEmpty(filter.Search) ? await usersQueryable.CountAsync() : usersQueryable.Count();
                if (count == 0)
                {
                    return new WrapperModel<UserDTO> { Count = count, DataList = new List<UserDTO>() };
                }
            }

            if (filter.Skip.HasValue)
            {
                usersQueryable = usersQueryable.Skip(filter.Skip.Value);
            }

            if (filter.Take.HasValue)
            {
                usersQueryable = usersQueryable.Take(filter.Take.Value);
            }

            IEnumerable<User> users = Enumerable.Empty<User>();
            users = string.IsNullOrEmpty(filter.Search) ? await usersQueryable.ToListAsync() : usersQueryable.ToList();
            List<UserDTO> userDTOs = users.Select(_mapper.Map<UserDTO>).ToList();

            if (!string.IsNullOrEmpty(filter.Expand))
            {
                string expand = filter.Expand.ToLower();
                if (expand.Contains("userprofile.followed"))
                {

                    List<UserFollower> userProfileFollowers = await _context.UserFollowers
                        .Where(f => f.FollowerId == userId).AsNoTracking().ToListAsync();
                    var userIds = userDTOs.Select(x => x.Id).ToList();
                    var followers = await _context.UserFollowers.Where(f => userIds.Contains(f.FollowedId)).ToListAsync();
                    for (int i = 0; i < userDTOs.Count; i++)
                    {
                        if (userDTOs[i].UserProfile != null)
                        {
                            userDTOs[i].UserProfile.IsFollowed = userProfileFollowers.Any(f => f.FollowedId.Equals(userDTOs[i].Id));
                            userDTOs[i].UserProfile.FollowersCount = followers.Count(f => f.FollowedId == userDTOs[i].Id);
                        }
                    }
                }
            }

            return new WrapperModel<UserDTO> { Count = count, DataList = userDTOs };
        }
        public async Task<UserDTO?> GetUserAsync(int id, string? expand = null, bool accessPrivateInfo = false)
        {
            var usersQueryable = ExpandSortUsers(_context.Users.AsNoTracking().AsSplitQuery(), expand, accessPrivateInfo: accessPrivateInfo);
            var user = await usersQueryable.FirstOrDefaultAsync(p => p.Id == id && p.StatusId != Enums.UserStatus.Deleted);
            return _mapper.Map<UserDTO>(user);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="companyId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<UserDTO?> GetSPAdminByCompany(int companyId, int? userId)
        {
            var userQueryable = _context.Users.Where(x => x.CompanyId == companyId).Include(y => y.Roles).Where(x => x.StatusId == Enums.UserStatus.Active || x.StatusId == Enums.UserStatus.Onboard);
            if (userId == 0)
            {
                userQueryable = userQueryable.Where(x => x.Roles.Any(x => x.RoleId == (int)RoleType.SPAdmin));
            }
            else
            {
                userQueryable = userQueryable.Where(x => x.Roles.Any(x => x.UserId != userId && x.RoleId == (int)RoleType.SPAdmin));
            }
            var user = await userQueryable.FirstOrDefaultAsync();
            return _mapper.Map<UserDTO>(user);
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            var user = await _context.Users.Where(u => u.Username == username)
              .Include(u => u.Permissions)
              .Include(u => u.Roles).ThenInclude(r => r.Role).ThenInclude(r => r.Permissions)
              .Include(u => u.UserProfile)
              .AsNoTracking()
              .FirstOrDefaultAsync();

            return user;
        }

        public async Task<bool> UpdateUserEmailPreference(int userId, EmailAlertCategory emailAlertCategory, EmailAlertFrequency? frequency)
        {
            UserEmailAlert? userEmailAlerts = await _context.UserEmailAlerts.FirstOrDefaultAsync(x => x.UserId == userId && x.EmailAlertId == (int)emailAlertCategory);
            if (userEmailAlerts != null)
            {
                try
                {
                    userEmailAlerts.Frequency = (EmailAlertFrequency)frequency;
                    _context.SaveChanges();
                    return true;
                }
                catch (Exception ex)
                {
                    throw new CustomException(CoreErrorMessages.ErrorOnSaving, ex);
                }
            }
            else
            {
                throw new CustomException(CoreErrorMessages.EntityNotFound);
            }
        }

        public async Task<User?> GetUserByUserIdAsync(int userId)
        {
            var user = await _context.Users.Where(u => u.Id == userId)
              .Include(email => email.UserEmailAlerts).ThenInclude(b => b.EmailAlert)
              .AsNoTracking()
              .Where(x => x.UserEmailAlerts.Any(uea => uea.EmailAlert.Id == ((int)EmailAlertCategory.Summary) && uea.Frequency != EmailAlertFrequency.Off))
              .FirstOrDefaultAsync();
            return user;
        }

        public async Task<int> CreateUpdateUserAsync(int id, UserDTO modelDTO, IEnumerable<EmailAlertDTO> emailAlertsDTO)
        {
            var userInfoOfExistingSPAdminOfCompany = new User();

            bool isEdit = id > 0;
            var model = new User();
            if (isEdit)
            {
                model = _context.Users.SingleOrDefault(b => b.Id == id);
                if (model == null)
                    throw new CustomException($"{CoreErrorMessages.ErrorOnSaving} User does not exist.");
            }
            int userId = isEdit ? id : 0;
            bool isStatusChange = modelDTO.StatusId != (int)model.StatusId;
            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    _mapper.Map(modelDTO, model);
                    model.Id = userId;
                    if (!isEdit)
                    {
                        _context.Users.Add(model);
                        _context.SaveChanges();
                        userId = model.Id;
                    }


                    //Edit User ; update status from active to Inactive for the selected user
                    if (isStatusChange && modelDTO.StatusId == (int)UserStatus.Inactive && modelDTO.Roles.Any(x => x.Id == (int)RoleType.SPAdmin))
                    {
                        _context.RemoveRange(_context.UserRoles.Where(a => a.UserId == id && a.RoleId == (int)RoleType.SPAdmin));
                        _context.UserRoles.AddRange(new UserRole() { UserId = id, RoleId = (int)RoleType.SolutionProvider });

                        _context.RemoveRange(_context.UserPermissions.Where(a => a.UserId == id && a.PermissionId != PermissionType.ProjectManagementOwn));
                    }
                    else
                    {

                        // Remove Roles and Permissions
                        _context.RemoveRange(_context.UserRoles.Where(a => a.UserId == userId));
                        _context.RemoveRange(_context.UserPermissions.Where(a => a.UserId == userId));

                        // Add new Roles and Permissions
                        IEnumerable<int> roleIds = modelDTO.Roles.Select(item => item.Id).Append((int)RoleType.All).Distinct();
                        _context.UserRoles.AddRange(roleIds.Select(id => new UserRole() { UserId = userId, RoleId = id }));
                        _context.UserPermissions.AddRange(modelDTO.Permissions.Select(item => new UserPermission() { UserId = userId, PermissionId = (PermissionType)item.Id }));

                        //Update the role of existing SP Admin to SP User
                        if (modelDTO.Roles.Any(x => x.Id == (int)RoleType.SPAdmin))
                        {
                            var userQueryable = _context.Users.Where(x => x.CompanyId == modelDTO.CompanyId).Include(y => y.Roles).
                              Where(x => x.StatusId == Enums.UserStatus.Active || x.StatusId == Enums.UserStatus.Onboard);
                            if (id == 0)
                            {
                                userQueryable = userQueryable.Where(x => x.Roles.Any(x => x.RoleId == (int)RoleType.SPAdmin));
                            }
                            else
                            {
                                userQueryable = userQueryable.Where(x => x.Roles.Any(x => x.UserId != id && x.RoleId == (int)RoleType.SPAdmin));
                            }
                            userInfoOfExistingSPAdminOfCompany = await userQueryable.FirstOrDefaultAsync();
                        }
                        if (userInfoOfExistingSPAdminOfCompany?.Id > 0)
                        {
                            _context.RemoveRange(_context.UserRoles.Where(a => a.UserId == userInfoOfExistingSPAdminOfCompany.Id && a.RoleId == (int)RoleType.SPAdmin));
                            _context.UserRoles.AddRange(new UserRole() { UserId = userInfoOfExistingSPAdminOfCompany.Id, RoleId = (int)RoleType.SolutionProvider });

                            _context.RemoveRange(_context.UserPermissions.Where(a => a.UserId == userInfoOfExistingSPAdminOfCompany.Id && a.PermissionId != PermissionType.ProjectManagementOwn));
                        }
                    }

                    if (!isEdit)
                    {
                        var emailAlerts = await _context.EmailAlerts.ToListAsync();
                        _context.UserEmailAlerts.AddRange(emailAlerts.Select(ea => new UserEmailAlert
                        {
                            EmailAlertId = ea.Id,
                            Frequency = ea.Frequency,
                            UserId = userId,
                        }));
                    }

                    if (isEdit && emailAlertsDTO != null)
                    {
                        IEnumerable<UserEmailAlert> userEmailAlerts = await _context.UserEmailAlerts.Include(uea => uea.EmailAlert)
                        .Where(uea => uea.UserId == userId && emailAlertsDTO.Select(d => d.Id).Contains(uea.Id))
                        .ToListAsync();

                        foreach (UserEmailAlert alert in userEmailAlerts)
                        {
                            EmailAlertDTO? alertDTO = emailAlertsDTO.SingleOrDefault(d => d.Id == alert.Id);
                            if (alertDTO != null)
                            {
                                alert.Frequency = alertDTO.Frequency;
                            }
                        }
                    }

                    await _context.SaveChangesAsync();
                    if (isStatusChange)
                        await UpdateCompanyDomainForUser(model.Email, model.CompanyId);
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    _logger.LogInformation($"User Update : exception {ex.Message} : innerexecption {ex.InnerException?.Message}");
                    transaction.Rollback();
                    throw new CustomException(CoreErrorMessages.ErrorOnSaving, ex);
                }
            }
            return userId;
        }

        public async Task<UserDTO?> PatchUserAsync(int userId, JsonPatchDocument patchDoc)
        {
            User? user = await _context.Users.Include(x => x.Roles).SingleOrDefaultAsync(t => t.Id == userId);
            if (user != null)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {
                    try
                    {
                        patchDoc.ApplyTo(user);

                        //add code to remove role and permission for SP admin and then add role for SP user alone.
                        if (user.Roles.Any(x => x.RoleId == (int)RoleType.SPAdmin) && (user.StatusId == UserStatus.Inactive || user.StatusId == UserStatus.Deleted))
                        {
                            _context.RemoveRange(_context.UserRoles.Where(a => a.UserId == userId && a.RoleId == (int)RoleType.SPAdmin));
                            _context.UserRoles.AddRange(new UserRole() { UserId = userId, RoleId = (int)RoleType.SolutionProvider });

                            _context.RemoveRange(_context.UserPermissions.Where(a => a.UserId == userId && a.PermissionId != PermissionType.ProjectManagementOwn));
                        }

                        await _context.SaveChangesAsync();
                        transaction.Commit();


                        await UpdateCompanyDomainForUser(user.Email, user.CompanyId);

                        return _mapper.Map<UserDTO>(user);
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

        public async Task RequestToDeleteUserAsync(int userId, string userName)
        {
            User? user = await _context.Users.Include(x => x.Roles).SingleOrDefaultAsync(t => t.Id == userId);
            if (user != null)
            {
                using (var transaction = _context.Database.BeginTransaction())
                {

                    try
                    {
                        user.RequestDeleteDate = DateTime.UtcNow;

                        //add code to remove role and permission for SP admin and then add role for SP user alone.
                        if (user.Roles.Any(x => x.RoleId == (int)RoleType.SPAdmin))
                        {
                            _context.RemoveRange(_context.UserRoles.Where(a => a.UserId == userId && a.RoleId == (int)RoleType.SPAdmin));
                            _context.UserRoles.AddRange(new UserRole() { UserId = userId, RoleId = (int)RoleType.SolutionProvider });

                            _context.RemoveRange(_context.UserPermissions.Where(a => a.UserId == userId && a.PermissionId != PermissionType.ProjectManagementOwn));
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
            }
            else
            {
                throw new CustomException(CoreErrorMessages.EntityNotFound);
            }
        }

        public async Task DeleteUserAsync(int id)
        {
            User? user = await _context.Users.SingleOrDefaultAsync(t => t.Id == id);
            if (user == null)
                throw new CustomException(CoreErrorMessages.EntityNotFound);

            string userName = string.Copy(user.Username);
            string email = user.Email;
            int companyId = user.CompanyId;

            try
            {
                user.FirstName = "Deleted";
                user.LastName = "User";
                user.Username = Guid.NewGuid().ToString();
                user.Email = Guid.NewGuid() + "@se.com";
                user.AzureId = string.Empty;
                user.ImageName = null;
                Company? company = await _context.Companies.SingleOrDefaultAsync(t => t.Name.ToLower().Equals("deleted company"));
                if (company == null)
                    throw new Exception($"We are missing 'Deleted Company' in DB.");

                user.CompanyId = company.Id;
                user.StatusId = Enums.UserStatus.Deleted;
                UserProfile? userProfile = await _context.UserProfiles.SingleOrDefaultAsync(t => t.UserId == user.Id);
                if (userProfile != null)
                {
                    userProfile.JobTitle = string.Empty;
                    userProfile.LinkedInUrl = string.Empty;
                    userProfile.About = string.Empty;
                    userProfile.StateId = null;
                }

                List<Project> projects = await _context.Projects.Where(f => f.OwnerId.Equals(user.Id)
                    && f.StatusId.Equals(Enums.ProjectStatus.Active)).ToListAsync();
                projects.ForEach(a => a.StatusId = Core.Enums.ProjectStatus.Inactive);

                _context.RemoveRange(_context.UserFollowers.Where(a => a.FollowedId.Equals(user.Id) || a.FollowerId.Equals(user.Id)));
                _context.RemoveRange(_context.CompanyFollowers.Where(a => a.FollowerId.Equals(user.Id)));
                _context.RemoveRange(_context.SavedArticles.Where(a => a.UserId.Equals(user.Id)));
                _context.RemoveRange(_context.SavedForums.Where(a => a.UserId.Equals(user.Id)));
                _context.RemoveRange(_context.SavedProjects.Where(a => a.UserId.Equals(user.Id)));
                _context.RemoveRange(_context.UserNotifications.Where(a => a.UserId.Equals(user.Id)));
                _context.RemoveRange(_context.ArticleViews.Where(a => a.UserId.Equals(user.Id)));
                _context.RemoveRange(_context.ProjectViews.Where(a => a.UserId.Equals(user.Id)));
                _context.RemoveRange(_context.UserEmailAlerts.Where(a => a.UserId.Equals(user.Id)));

                _context.RemoveRange(_context.ToolsPinned.Where(a => a.UserId.Equals(user.Id)));
                _context.RemoveRange(_context.UserProfileUrlLinks.Where(a => a.UserProfileId.Equals(user.Id)));
                _context.RemoveRange(_context.UserProfileRegions.Where(a => a.UserProfileId.Equals(user.Id)));
                _context.RemoveRange(_context.UserProfileCategories.Where(a => a.UserProfileId.Equals(user.Id)));
                _context.RemoveRange(_context.UserPermissions.Where(a => a.UserId.Equals(user.Id)));
                _context.RemoveRange(_context.EventAttendees.Where(a => a.UserId.Equals(user.Id)));

                _context.RemoveRange(_context.EventInvitedUsers.Where(a => a.UserId.Equals(user.Id)));
                _context.RemoveRange(_context.MessageLikes.Where(a => a.UserId.Equals(user.Id)));
                _context.RemoveRange(_context.DiscussionFollowers.Where(a => a.UserId.Equals(user.Id)));

                List<EventModerator> eventModerator = await _context.EventModerators.Where(f => f.UserId.Equals(user.Id)).ToListAsync();
                eventModerator.ForEach(a => { a.Name = "Deleted User"; a.Company = "Deleted Company"; });

                await _context.SaveChangesAsync();
                await UpdateCompanyDomainForUser(email, companyId);

                _cache.Remove(userName);
            }
            catch (Exception ex)
            {
                throw new CustomException(CoreErrorMessages.ErrorOnRemoving, ex);
            }
        }

        public async Task CreateUserFollowerAsync(int followerId, int followedId)
        {
            if (followedId == followerId)
                throw new CustomException(CoreErrorMessages.ProhibitedFollowYourself);

            if (_context.Users.SingleOrDefault(b => b.Id == followedId) == null)
                throw new CustomException($"{CoreErrorMessages.ErrorOnSaving} User does not exist.");

            if (_context.UserFollowers.Where(i => i.FollowedId.Equals(followedId) && i.FollowerId.Equals(followerId)).Any())
                throw new CustomException(CoreErrorMessages.FollowerExists);

            try
            {
                var model = new UserFollower { FollowerId = followerId, FollowedId = followedId };
                _context.UserFollowers.Add(model);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new CustomException(CoreErrorMessages.ErrorOnSaving, ex);
            }
        }

        public async Task RemoveUserFollowerAsync(int followerId, int followedId)
        {
            if (_context.Users.SingleOrDefault(b => b.Id == followedId) == null)
                throw new CustomException($"{CoreErrorMessages.ErrorOnSaving} User does not exist.");

            var model = await _context.UserFollowers.SingleOrDefaultAsync(sp => sp.FollowerId == followerId && sp.FollowedId == followedId);
            if (model != null)
            {
                try
                {
                    _context.UserFollowers.Remove(model);
                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw new CustomException(CoreErrorMessages.ErrorOnRemoving, ex);
                }
            }
        }

        public async Task<List<int>> GetAdminUsersIdsAsync()
        {
            return await _context.Users.AsNoTracking()
                .Where(u => u.Roles.Any(r => r.RoleId == (int)RoleType.Admin || r.RoleId == (int)RoleType.SystemOwner)
                            && u.StatusId == Enums.UserStatus.Active)
                .Select(u => u.Id)
                .ToListAsync();
        }

        public async Task<bool> UpdateOnboardUserStatus()
        {
            try
            {
                var onboradUsers = await _context.Users
                    .Where(u => u.CreatedOn.Value.AddDays(20) <= DateTime.UtcNow && u.StatusId == Enums.UserStatus.Onboard)
                    .AsNoTracking()
                    .ToListAsync();

                if (onboradUsers.Any())
                {
                    onboradUsers.ForEach(u => u.StatusId = Enums.UserStatus.Expired);
                    _context.Users.UpdateRange(onboradUsers);
                    await _context.SaveChangesAsync();
                }

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogInformation($"Onborad User Update : exception {ex.Message} : innerexecption {ex.InnerException?.Message}");
                return false;
            }
        }

        #endregion User Management

        #region User Validation

        public bool IsUserEmailExist(int userId, string userEmail)
        {
            var user = _context.Users.Where(p => p.Email.Equals(userEmail) && p.Id.Equals(userId)).ToList();
            return user.Any();
        }

        public bool IsUserNameExist(int userId, string username)
        {
            var user = _context.Users.Where(p => p.Username.Equals(username) && p.Id.Equals(userId)).ToList();
            return user.Any();
        }

        public bool IsUserIdExist(int userId)
        {
            var user = _context.Users.Where(p => p.Id.Equals(userId)).ToList();
            return user.Any();
        }

        public bool IsUserIdInProfileExist(int userId)
        {
            var userProfile = _context.UserProfiles.Where(p => p.UserId.Equals(userId)).ToList();
            return userProfile.Any();
        }

        public bool IsUserNameOrEmailExists(string email, int? exceptId = null)
        {
            return _context.Users
                .Any(u => (u.Email.ToLower() == email.ToLower() || u.Username.ToLower() == email.ToLower())
                    && (exceptId == null || u.Id != exceptId));
        }

        public bool IsSolutionProviderUserExist(List<int> userIds)
        {
            return _context.Users.Any(p => userIds.Contains(p.Id) && p.Roles.Any(r => r.RoleId.Equals((int)Common.Enums.RoleType.SolutionProvider) || r.RoleId.Equals((int)Common.Enums.RoleType.SPAdmin)));
        }

        public bool IsInternalCorporationUser(int userId)
        {
            return _context.Users.Any(p => p.Id.Equals(userId)
                && p.Roles.Any(r => r.RoleId.Equals((int)Common.Enums.RoleType.Internal))
                    && p.Company.TypeId.Equals(CompanyType.Corporation));
        }

        #endregion User Validation

        #region Private Method
        // Method to update Company Domain based on active users of company
        private async Task UpdateCompanyDomainForUser(string userEmail, int companyId)
        {
            MailAddress address = new MailAddress(userEmail);
            string domainName = address.Host;


            List<User> users = await _context.Users.Where(p => (p.Email.Contains(domainName) && p.CompanyId == companyId)).ToListAsync();
            var companyDomain = await _context.CompanyDomains.FirstOrDefaultAsync(cd => cd.DomainName == domainName && cd.CompanyId == companyId);
            if (users.Any() && companyDomain != null)
            {
                companyDomain.IsActive = users.Any(u => u.StatusId == Enums.UserStatus.Active);
            }
            else
            {
                if (companyDomain != null)
                    _context.CompanyDomains.Remove(companyDomain);
            }
            await _context.SaveChangesAsync();

        }
        #endregion Private Method

        public async Task<List<SkillsByCategoryDTO>> GetSkillsByCategory(List<int> roleIds, int userId)
        {
            List<SkillsByCategoryDTO> categoryVsSkills = new List<SkillsByCategoryDTO>();
            if (roleIds.Contains((int)RoleType.SolutionProvider) || (roleIds.Contains((int)RoleType.SPAdmin)))
            {
                var categoriesId = await _context.Users.Include(z => z.Company).ThenInclude(y => y.Categories).Where(x => x.Id == userId).SelectMany(x => x.Company.Categories).
                                   Select(z => z.CategoryId).ToListAsync();
                if (categoriesId.Count > 0)
                {
                    var result = await _context.SkillsByCategory.Include(x => x.Skills).Include(x => x.Category).Where(sc => sc.Skills.RoleType == RoleType.SolutionProvider && 
                                        categoriesId.Contains(sc.CategoryId)).OrderBy(sc => sc.Category.Name).ThenBy(sc => sc.Skills.Name).ToListAsync();
                    categoryVsSkills = _mapper.Map<List<SkillsByCategoryDTO>>(result);

                }
            }
            else if (roleIds.Contains((int)RoleType.Corporation))
            {
                var result = await _context.Skills.Where(s => s.RoleType == RoleType.Corporation).OrderBy(x=>x.Name).ToListAsync();
                categoryVsSkills = _mapper.Map<List<SkillsByCategoryDTO>>(result);
               
            }


            return categoryVsSkills;
        }
    }
}
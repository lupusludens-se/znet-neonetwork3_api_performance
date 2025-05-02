using Microsoft.AspNetCore.JsonPatch;
using SE.Neo.Common.Enums;
using SE.Neo.Common.Models.EmailAlert;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Common.Models.User;
using SE.Neo.Common.Models.UserProfile;
using SE.Neo.Core.Entities;
using SE.Neo.Core.Services.Interfaces;

namespace SE.Neo.Core.Services.Decorators
{
    public abstract class AbstractUserServiceDecorator : IUserService
    {
        protected readonly IUserService _userService;

        public AbstractUserServiceDecorator(IUserService userService)
        {
            _userService = userService;
        }

        public virtual async Task<int> CreateUpdateUserAsync(int id, UserDTO modelDTO, IEnumerable<EmailAlertDTO> emailDTO)
        {
            return await _userService.CreateUpdateUserAsync(id, modelDTO, emailDTO);
        }

        public virtual async Task<UserDTO?> PatchUserAsync(int id, JsonPatchDocument patchDoc)
        {
            return await _userService.PatchUserAsync(id, patchDoc);
        }
        public virtual async Task<UserDTO?> GetUserAsync(int id, string? expand = null, bool accessPrivateInfo = false)
        {
            return await _userService.GetUserAsync(id, expand, accessPrivateInfo);
        }
        public virtual async Task<UserDTO?> GetSPAdminByCompany(int companyId, int? userId)
        {
            return await _userService.GetSPAdminByCompany(companyId, userId);
        }

        public virtual async Task<WrapperModel<UserDTO>> GetUsersAsync(BaseSearchFilterModel filter, int userId, IEnumerable<int> userRoleIds, int companyId, bool isOwnCompanyUsersRequest = false, bool accessPrivateInfo = false)
        {
            return await _userService.GetUsersAsync(filter, userId, userRoleIds, companyId, isOwnCompanyUsersRequest, accessPrivateInfo);
        }
        public virtual async Task<User?> GetUserByUserIdAsync(int userId)
        {
            return await _userService.GetUserByUserIdAsync(userId);
        }

        public virtual async Task<bool> UpdateUserEmailPreference(int userId, EmailAlertCategory emailAlertCategory, EmailAlertFrequency? frequency)
        {
            return await _userService.UpdateUserEmailPreference(userId, emailAlertCategory, frequency);
        }

        public virtual async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _userService.GetUserByUsernameAsync(username);
        }

        public virtual async Task DeleteUserAsync(int id)
        {
            await _userService.DeleteUserAsync(id);
        }

        public virtual async Task RequestToDeleteUserAsync(int id, string userName)
        {
            await _userService.RequestToDeleteUserAsync(id, userName);
        }

        public virtual async Task<List<int>> GetAdminUsersIdsAsync()
        {
            return await _userService.GetAdminUsersIdsAsync();
        }

        public virtual async Task CreateUserFollowerAsync(int followerId, int followedId)
        {
            await _userService.CreateUserFollowerAsync(followerId, followedId);
        }

        public virtual async Task RemoveUserFollowerAsync(int followerId, int followedId)
        {
            await _userService.RemoveUserFollowerAsync(followerId, followedId);
        }

        public virtual bool IsUserIdExist(int userId)
        {
            return _userService.IsUserIdExist(userId);
        }

        public virtual bool IsUserIdInProfileExist(int userId)
        {
            return _userService.IsUserIdInProfileExist(userId);
        }

        public virtual bool IsUserNameExist(int userId, string username)
        {
            return _userService.IsUserNameExist(userId, username);
        }

        public virtual bool IsUserEmailExist(int userId, string userEmail)
        {
            return _userService.IsUserEmailExist(userId, userEmail);
        }

        public virtual bool IsUserNameOrEmailExists(string email, int? exceptId = null)
        {
            return _userService.IsUserNameOrEmailExists(email, exceptId);
        }

        public virtual bool IsSolutionProviderUserExist(List<int> userIds)
        {
            return _userService.IsSolutionProviderUserExist(userIds);
        }

        public virtual bool IsInternalCorporationUser(int userId)
        {
            return _userService.IsInternalCorporationUser(userId);
        }

         public virtual async Task<bool> UpdateOnboardUserStatus()
        {
            return await _userService.UpdateOnboardUserStatus();
        }
        public virtual async Task<List<SkillsByCategoryDTO>> GetSkillsByCategory(List<int> roleIds, int userId)
        {
            return await _userService.GetSkillsByCategory(roleIds, userId);
        }
    }
}
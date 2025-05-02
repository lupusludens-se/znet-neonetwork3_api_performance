using Microsoft.AspNetCore.JsonPatch;
using SE.Neo.Common.Enums;
using SE.Neo.Common.Models.EmailAlert;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Common.Models.User;
using SE.Neo.Common.Models.UserProfile;
using SE.Neo.Core.Entities;

namespace SE.Neo.Core.Services.Interfaces
{
    public interface IUserService
    {
        Task<WrapperModel<UserDTO>> GetUsersAsync(BaseSearchFilterModel filter, int userId, IEnumerable<int>? userRoleIds = null, int companyId = 0, bool isOwnCompanyUsersRequest = false, bool accessPrivateInfo = false);

        Task<UserDTO?> GetUserAsync(int id, string? expand = null, bool accessPrivateInfo = false);

        Task<User?> GetUserByUsernameAsync(string username);

        Task<User?> GetUserByUserIdAsync(int userId);

        Task<int> CreateUpdateUserAsync(int id, UserDTO modelDTO, IEnumerable<EmailAlertDTO>? emailAlertsDTO);

        Task<UserDTO?> PatchUserAsync(int id, JsonPatchDocument patchDoc);

        Task DeleteUserAsync(int id);

        Task RequestToDeleteUserAsync(int id, string userName);

        Task CreateUserFollowerAsync(int followerId, int followedId);

        Task RemoveUserFollowerAsync(int followerId, int followedId);

        bool IsUserEmailExist(int userId, string userEmail);

        bool IsUserNameExist(int userId, string username);

        bool IsUserIdInProfileExist(int userId);

        bool IsUserIdExist(int userId);

        bool IsSolutionProviderUserExist(List<int> userIds);

        Task<List<int>> GetAdminUsersIdsAsync();

        bool IsUserNameOrEmailExists(string email, int? exceptId = null);

        bool IsInternalCorporationUser(int userId);
        Task<bool> UpdateUserEmailPreference(int userId, EmailAlertCategory emailAlertCategory, EmailAlertFrequency? frequency);
        Task<UserDTO?> GetSPAdminByCompany(int companyId, int? userId);
        
        Task<bool> UpdateOnboardUserStatus();
        Task<List<SkillsByCategoryDTO>> GetSkillsByCategory(List<int> roleIds, int userId);
    }
}
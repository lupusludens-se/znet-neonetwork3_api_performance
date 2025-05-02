using Microsoft.AspNetCore.JsonPatch;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Common.Models.UserProfile;
using SE.Neo.WebAPI.Models.User;

namespace SE.Neo.WebAPI.Services.Interfaces
{
    public interface IUserApiService
    {
        Task<WrapperModel<UserResponse>> GetUsersAsync(BaseSearchFilterModel filter, UserModel userDetails, bool isOwnCompanyUsersRequest = false, bool accessPrivateInfo = false);

        Task<int> ExportUsersAsync(BaseSearchFilterModel filter, MemoryStream stream, UserModel userDetails, bool isSPAdminRequest = false);

        Task<UserResponse?> GetUserAsync(int id, string? expand, bool accessPrivateInfo = false);

        Task<int> CreateUpdateUserAsync(int id, UserRequest userViewModel);

        Task<bool> PatchUserAsync(int id, JsonPatchDocument patchDoc, UserModel userDetails, bool isAdminRequest = false);

        Task RequestToDeleteUserAsync(int id, string userName);

        Task<bool> DeleteUserAsync(int id, UserModel userDetails);

        Task<UserModel?> GetUserModelByUsernameAsync(string username);

        Task CreateUserFollowerAsync(int followerId, int followedId);

        Task RemoveUserFollowerAsync(int followerId, int followedId);


        Task<UserResponse?> GetSPAdminByCompany(int companyId, int? userId);

        bool IsInternalCorporationUser(int userId);

        bool IsNewUser(int userId);

        Task<bool> UpdateOnboardUserStatus();
        Task<List<SkillsByCategoryResponse>> GetSkillsByCategory(UserModel user);
    }
}
using SE.Neo.Common.Models.Shared;
using SE.Neo.WebAPI.Models.User;

namespace SE.Neo.WebAPI.Services.Interfaces
{
    public interface IUserPendingApiService
    {
        Task<WrapperModel<UserPendingListItemResponse>> GetUserPendingsAsync(ExpandOrderModel filter);

        Task<UserPendingItemResponse?> GetUserPendingAsync(int id, string? expand);

        Task<int> CreateUpdateUserPendingAsync(UserPendingRequest model, int id = 0);

        Task<int> ApproveUserPending(int id);

        Task<bool> DenyUserPending(int id, bool isDenied);

        Task<int> GetPendingUserCount();

        Task DeleteDeniedUserPendingsAsync();
    }
}

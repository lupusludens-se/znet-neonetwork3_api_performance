using SE.Neo.Common.Models.Shared;
using SE.Neo.Common.Models.User;

namespace SE.Neo.Core.Services.Interfaces
{
    public interface IUserPendingService
    {
        Task<WrapperModel<UserPendingDTO>> GetUserPendingsAsync(ExpandOrderModel filter);

        Task<UserPendingDTO?> GetUserPendingAsync(int id, string? expand = null);

        Task<UserPendingDTO> CreateUpdateUserPendingAsync(UserPendingDTO modelDTO);

        Task<bool> DeleteUserPendingAsync(int id);

        bool IsUserPendingExist(string email, int? exceptId = null);

        Task<UserDTO> ApproveUserPendingAsync(UserPendingDTO userPendingDTO);

        Task<bool> DenyUserPendingAsync(int id, bool isDenied);

        Task<int> GetPendingUserCountAsync();

        Task DeleteDeniedUserPendingsAsync();
    }
}

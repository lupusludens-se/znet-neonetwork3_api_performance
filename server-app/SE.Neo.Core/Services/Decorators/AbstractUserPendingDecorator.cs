using SE.Neo.Common.Models.Shared;
using SE.Neo.Common.Models.User;
using SE.Neo.Core.Services.Interfaces;

namespace SE.Neo.Core.Services.Decorators
{
    public class AbstractUserPendingDecorator : IUserPendingService
    {
        protected readonly IUserPendingService _userPendingServiceService;

        public AbstractUserPendingDecorator(
            IUserPendingService userPendingServiceService)
        {
            _userPendingServiceService = userPendingServiceService;
        }

        public virtual async Task<UserDTO> ApproveUserPendingAsync(UserPendingDTO userPendingDTO)
        {
            return await _userPendingServiceService.ApproveUserPendingAsync(userPendingDTO);
        }

        public virtual async Task<UserPendingDTO> CreateUpdateUserPendingAsync(UserPendingDTO modelDTO)
        {
            return await _userPendingServiceService.CreateUpdateUserPendingAsync(modelDTO);
        }

        public virtual async Task DeleteDeniedUserPendingsAsync()
        {
            await _userPendingServiceService.DeleteDeniedUserPendingsAsync();
        }

        public virtual async Task<bool> DeleteUserPendingAsync(int id)
        {
            return await _userPendingServiceService.DeleteUserPendingAsync(id);
        }

        public virtual async Task<bool> DenyUserPendingAsync(int id, bool isDenied)
        {
            return await _userPendingServiceService.DenyUserPendingAsync(id, isDenied);
        }

        public virtual async Task<int> GetPendingUserCountAsync()
        {
            return await _userPendingServiceService.GetPendingUserCountAsync();
        }

        public virtual async Task<UserPendingDTO?> GetUserPendingAsync(int id, string? expand = null)
        {
            return await _userPendingServiceService.GetUserPendingAsync(id, expand);
        }

        public virtual async Task<WrapperModel<UserPendingDTO>> GetUserPendingsAsync(ExpandOrderModel filter)
        {
            return await _userPendingServiceService.GetUserPendingsAsync(filter);
        }

        public virtual bool IsUserPendingExist(string email, int? exceptId = null)
        {
            return _userPendingServiceService.IsUserPendingExist(email, exceptId);
        }
    }
}

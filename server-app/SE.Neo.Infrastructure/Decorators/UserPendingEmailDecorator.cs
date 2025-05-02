using SE.Neo.Common.Models.User;
using SE.Neo.Core.Services.Decorators;
using SE.Neo.Core.Services.Interfaces;
using SE.Neo.Infrastructure.Services.Interfaces;

namespace SE.Neo.Infrastructure.Decorators
{
    public class UserPendingEmailDecorator : AbstractUserPendingDecorator
    {
        private readonly IEmailNotificationService _emailNotificationService;

        public UserPendingEmailDecorator(
            IUserPendingService userPendingService,
            IEmailNotificationService emailNotificationService
           ) : base(userPendingService)
        {
            _emailNotificationService = emailNotificationService;
        }

        public override async Task<UserDTO> ApproveUserPendingAsync(UserPendingDTO userPendingDTO)
        {
            UserDTO userDTO = await base.ApproveUserPendingAsync(userPendingDTO);

            // TODO: check whether we need here await
            await _emailNotificationService.CompleteRegistrationAsync(userDTO.Username, userDTO.FirstName);

            return userDTO;
        }
    }
}

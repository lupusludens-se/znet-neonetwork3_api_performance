using Microsoft.Extensions.Options;
using SE.Neo.Common.Models.EmailAlert;
using SE.Neo.Common.Models.User;
using SE.Neo.Core.Services.Decorators;
using SE.Neo.Core.Services.Interfaces;
using SE.Neo.Infrastructure.Configs;
using SE.Neo.Infrastructure.Services.Interfaces;

namespace SE.Neo.Infrastructure.Decorators
{
    public class UserServiceEmailDecorator : AbstractUserServiceDecorator
    {
        private readonly IEmailNotificationService _emailNotificationService;
        private readonly IEmailService _emailService;
        private readonly DeleteUserEmailConfig _emailConfig;

        public UserServiceEmailDecorator(
            IUserService userService,
            IEmailNotificationService emailNotificationService,
            IEmailService emailService,
            IOptions<DeleteUserEmailConfig> emailConfig)
            : base(userService)
        {
            _emailNotificationService = emailNotificationService;
            _emailService = emailService;
            _emailConfig = emailConfig.Value;
        }

        public override async Task<int> CreateUpdateUserAsync(int id, UserDTO userDTO, IEnumerable<EmailAlertDTO> emailAlertDTOs)
        {
            int userId = await base.CreateUpdateUserAsync(id, userDTO, emailAlertDTOs);

            if (id == 0)
            {
                // send email notification only for new user
                // TODO: check whether we need here await
                await _emailNotificationService.CompleteRegistrationAsync(userDTO.Username, userDTO.FirstName);
            }

            return userId;
        }

        public override async Task RequestToDeleteUserAsync(int id, string userName)
        {
            await base.RequestToDeleteUserAsync(id, userName);
            string htmlBody = $"Deletion request from user {userName}.";

            await _emailService.SendEmailAsync(_emailConfig.To, _emailConfig.Subject, htmlBody);
        }
    }
}
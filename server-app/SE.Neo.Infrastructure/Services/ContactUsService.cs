using Microsoft.Extensions.Options;
using SE.Neo.Infrastructure.Configs;
using SE.Neo.Infrastructure.Services.Interfaces;

namespace SE.Neo.Infrastructure.Services
{
    public class ContactUsService : IContactUsService
    {
        private readonly ContactUsEmailConfig _contactUsEmailConfig;
        private readonly IEmailService _emailService;

        public ContactUsService(IOptions<ContactUsEmailConfig> contactUsEmailConfigOptions, IEmailService emailService)
        {
            _emailService = emailService;
            _contactUsEmailConfig = contactUsEmailConfigOptions.Value;
        }

        public async Task SendContactUsMessageAsync(string htmlBody)
        {
            await _emailService.SendEmailAsync(_contactUsEmailConfig.To, _contactUsEmailConfig.Subject, htmlBody);
        }
    }
}
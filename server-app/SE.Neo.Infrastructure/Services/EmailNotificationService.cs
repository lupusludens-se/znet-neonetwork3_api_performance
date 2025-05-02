using Microsoft.Extensions.Options;
using SE.Neo.EmailTemplates.Models;
using SE.Neo.Infrastructure.Configs;
using SE.Neo.Infrastructure.Constants;
using SE.Neo.Infrastructure.Services.Interfaces;
using System.Web;

namespace SE.Neo.Infrastructure.Services
{
    public class EmailNotificationService : IEmailNotificationService
    {
        private readonly IEmailService _emailService;
        private readonly AzureB2CConfig _b2cAppConfig;
        private readonly AzureStorageConfig _azureStorageConfig;
        private readonly EmailAssetsConfig _emailAssetsConfig;

        public EmailNotificationService(
            IOptions<AzureB2CConfig> b2cAppConfigOptions,
            IOptions<AzureStorageConfig> azureStorageConfigOptions,
            IOptions<EmailAssetsConfig> emailAssetsConfigOptions,
            IEmailService emailService)
        {
            _emailService = emailService;
            _b2cAppConfig = b2cAppConfigOptions.Value;
            _azureStorageConfig = azureStorageConfigOptions.Value;
            _emailAssetsConfig = emailAssetsConfigOptions.Value;
        }

        public async Task CompleteRegistrationAsync(string username, string firstName)
        {
#if DEBUG
            const string linkTemplate = "{0}/{1}/oauth2/v2.0/authorize?client_id={2}&scope=openid%20profile%20offline_access&redirect_uri={3}&client-request-id=315f835b-61d2-4b19-a94d-530b0d67553b&response_mode=fragment&response_type=code&x-client-SKU=msal.js.browser&x-client-VER=2.22.0&x-client-OS=&x-client-CPU=&client_info=1&code_challenge=fbgtW3QkqECTPhA54ntrxLYAptu7_zt6WIwWJtCuWxs&code_challenge_method=S256&nonce=cb472ce7-c6ae-417f-baea-3c69e3d9f2ba&state=eyJpZCI6IjA2MjJhZjQyLTgxMzUtNGEzYy05NmVhLWY4ZWZkMDIwYzQ4NCIsIm1ldGEiOnsiaW50ZXJhY3Rpb25UeXBlIjoicmVkaXJlY3QifX0%3D";
#else
            const string linkTemplate = "{0}/oauth2/v2.0/authorize?p={1}&client_id={2}&nonce=defaultNonce&redirect_uri={3}&scope=openid&response_type=code&prompt=login";
#endif
            var emailTemplateModel = new CompleteRegistrationEmailTemplatedModel
            {
                LogoUrl = $"{_emailAssetsConfig.ZeigoLogo}",
                BoldFontLink = $"{_azureStorageConfig.CdnEndpoint}/{_emailAssetsConfig.AssetsBlobContainer}/{_emailAssetsConfig.BoldFont}",
                LightFontLink = $"{_azureStorageConfig.CdnEndpoint}/{_emailAssetsConfig.AssetsBlobContainer}/{_emailAssetsConfig.LightFont}",
                RegularFontLink = $"{_azureStorageConfig.CdnEndpoint}/{_emailAssetsConfig.AssetsBlobContainer}/{_emailAssetsConfig.RegularFont}",
                FirstName = firstName,
                Link = string.Format(linkTemplate,
                    $"{_b2cAppConfig.Instance}/{_b2cAppConfig.Domain}",
                    _b2cAppConfig.ReserPasswordPolicyId,
                    _b2cAppConfig.AppClientId,
                    HttpUtility.UrlEncode(_b2cAppConfig.RedirectUrl))
            };

            await _emailService.SendTemplatedEmailAsync(username, EmailSubjects.CompleteRegistration, emailTemplateModel);
        }
    }
}

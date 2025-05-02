using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SE.Neo.EmailAlertSender.Configs;
using SE.Neo.Infrastructure.Configs;

namespace SE.Neo.EmailAlertSender
{
    public static class ConfigServicesCollectionExtensions
    {
        public static IServiceCollection AddConfig(this IServiceCollection services, IConfiguration config)
        {
            services.AddOptions<AzureKeyVaultConfig>()
                .Bind(config.GetSection("AzureKeyVault"))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddOptions<AzureStorageConfig>()
                .Bind(config.GetSection("AzureStorage"))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddOptions<EmailAlertConfig>()
                .Bind(config.GetSection("EmailAlert"))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddOptions<AzureB2CConfig>()
                .Bind(config.GetSection("AzureAdB2C"))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddOptions<EmailConfig>()
                .Bind(config.GetSection("Email"))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddOptions<EmailAssetsConfig>()
                .Bind(config.GetSection("Email:Assets"))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddOptions<UnsubscribeSettingsConfig>()
             .Bind(config.GetSection("UnsubscribeSettings"))
             .ValidateDataAnnotations()
             .ValidateOnStart();

            services.AddOptions<BaseAppConfig>()
             .Bind(config.GetSection("BaseApp"))
             .ValidateDataAnnotations()
             .ValidateOnStart();

            services.AddOptions<SummaryEmailSettingsConfig>()
           .Bind(config.GetSection("SummaryEmailSettings"))
           .ValidateDataAnnotations()
           .ValidateOnStart();

            return services;
        }
    }
}
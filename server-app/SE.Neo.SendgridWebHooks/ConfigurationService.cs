using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SE.Neo.WebHooks.Configs;

namespace SE.Neo.WebHooks
{
    public static class ConfigurationService
    {
        public static IServiceCollection AddConfig(this IServiceCollection services, IConfiguration config)
        {
            services.AddOptions<AzureKeyVaultConfig>()
            .Bind(config.GetSection("AzureKeyVault"))
            .ValidateDataAnnotations()
            .ValidateOnStart();

            services.AddOptions<SettingsConfig>()
            .Bind(config.GetSection("Email"))
            .ValidateDataAnnotations()
            .ValidateOnStart();

            return services;
        }
    }
}
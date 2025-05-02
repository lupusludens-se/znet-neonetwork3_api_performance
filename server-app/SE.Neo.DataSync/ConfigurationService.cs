using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SE.Neo.DataSync.Configs;

namespace SE.Neo.DataSync
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
            .Bind(config.GetSection("Settings"))
            .ValidateDataAnnotations()
            .ValidateOnStart();

            return services;
        }
    }
}
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SE.Neo.Infrastructure.Configs;

namespace SE.Neo.CompanyLogoExporter
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

            services.AddOptions<GeneralFilesLimitationsConfig>()
                .Bind(config.GetSection("GeneralFilesLimitations"))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddOptions<AttachmentLimitationsConfig>()
                .Bind(config.GetSection("AttachmentLimitations:Initiative"))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            return services;
        }
    }
}
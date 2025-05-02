using SE.Neo.Core.Configs;
using SE.Neo.Infrastructure.Configs;

namespace SE.Neo.WebAPI.Extensions
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

            services.AddOptions<WordPressConfig>()
                .Bind(config.GetSection("WordPress"))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddOptions<DotDigitalConfig>()
               .Bind(config.GetSection("DotDigital"))
               .ValidateDataAnnotations()
               .ValidateOnStart();

            services.AddOptions<RecommenderSystemConfig>()
                .Bind(config.GetSection("RecommenderSystem"))
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

            services.AddOptions<MemoryCacheTimeStamp>()
                .Bind(config.GetSection("MemoryCacheTimeStamp"))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddOptions<EmailConfig>()
                .Bind(config.GetSection("Email"))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddOptions<SolarQuoteEmailConfig>()
                .Bind(config.GetSection("Email:SolarQuote"))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddOptions<GraphAPIConfig>()
                .Bind(config.GetSection("GraphAPI"))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddOptions<AzureB2CConfig>()
                .Bind(config.GetSection("AzureAdB2C"))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddOptions<EmailAssetsConfig>()
                .Bind(config.GetSection("Email:Assets"))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddOptions<ContactUsEmailConfig>()
                .Bind(config.GetSection("Email:ContactUs"))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddOptions<ScheduleDemoEmailConfig>()
               .Bind(config.GetSection("Email:ScheduleDemo"))
               .ValidateDataAnnotations()
               .ValidateOnStart();

            services.AddOptions<DeleteUserEmailConfig>()
                .Bind(config.GetSection("Email:DeleteUser"))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddOptions<AzureCognitiveSearchConfig>()
                .Bind(config.GetSection("AzureCognitiveSearch"))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddOptions<ProjectTagsConfig>()
                .Bind(config.GetSection("ProjectTags"))
                .ValidateDataAnnotations()
                .ValidateOnStart();

            services.AddOptions<PublicPlatformConfiguration>()
               .Bind(config.GetSection("PublicPlatformSettings"))
               .ValidateDataAnnotations()
               .ValidateOnStart();

            return services;
        }
    }
}
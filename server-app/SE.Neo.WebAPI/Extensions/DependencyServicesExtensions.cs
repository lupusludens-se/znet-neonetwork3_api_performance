using AutoMapper;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Options;
using SE.Neo.Core.Configs;
using SE.Neo.Core.Data;
using SE.Neo.Core.Factories;
using SE.Neo.Core.Factories.Interfaces;
using SE.Neo.Core.Services;
using SE.Neo.Core.Services.Interfaces;
using SE.Neo.Infrastructure.Configs;
using SE.Neo.Infrastructure.Decorators;
using SE.Neo.Infrastructure.Services;
using SE.Neo.Infrastructure.Services.Interfaces;
using SE.Neo.WebAPI.Handlers;
using SE.Neo.WebAPI.Models.Project;
using SE.Neo.WebAPI.Services;
using SE.Neo.WebAPI.Services.Interfaces;
using SE.Neo.WebAPI.Validators;

namespace SE.Neo.WebAPI.Extensions
{
    public static class DependencyServicesExtensions
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services)
        {
            // Base Services
            services.AddScoped<IAuthorizationMiddlewareResultHandler, NeoAuthorizationMiddlewareResultHandler>();
            services.AddScoped<IWordPressContentService, WordPressContentService>();
            services.AddScoped<IDotDigitalService, DotDigitalService>();
            services.AddHttpClient<IWordPressContentService, WordPressContentService>();
            services.AddHttpClient<IDotDigitalService, DotDigitalService>();
            services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();
            services.AddSingleton<IRazorViewEngine, RazorViewEngine>();
            services.AddSingleton<IRenderEmailTemplateService, RenderEmailTemplateService>();
            services.AddScoped<IEmailService, EmailService>();
            services.AddTransient<ICurrentUserResolverService, CurrentUserResolverService>();
            services.AddScoped<IAzureStorageBlobService, AzureStorageBlobService>();
            services.AddScoped<IFormFileValidationService, FormFileValidationService>();
            services.AddSingleton<BlobTypesFilesLimitationsConfig>();
            services.AddHttpClient<IGraphAPIService, GraphAPIService>();
            services.AddScoped<IContactUsService, ContactUsService>();
            services.AddScoped<IUnsubscribeEmailService, UnsubscribeEmailService>();

            // Core Services
            services.AddScoped<ICommonService, CommonService>();
            services.AddScoped<IProjectService>(sp => new ProjectServiceBlobDecorator(
                new ProjectService(
                    sp.GetService<ApplicationContext>(),
                    sp.GetService<IProjectDetailsFactory>(),
                    sp.GetService<ILogger<ProjectService>>(),
                    sp.GetService<IMapper>(),
                    sp.GetService<IDistributedCache>(),
                    sp.GetService<ICommonService>()),
                sp.GetService<IBlobServicesFacade>()));

            services.AddScoped<IConversationService>(sp =>
            {
                var applicationContext = sp.GetRequiredService<ApplicationContext>();
                var logger = sp.GetRequiredService<ILogger<ConversationService>>();
                var mapper = sp.GetRequiredService<IMapper>();
                var blobServicesFacade = sp.GetRequiredService<IBlobServicesFacade>();
                var conversationService = new ConversationService(applicationContext, logger, mapper);
                return new ConversationServiceBlobDecorator(conversationService, blobServicesFacade);
            });

            services.AddScoped<IBlobService, BlobService>();
            services.AddScoped<IBlobServicesFacade, BlobServicesFacade>();

            services.AddScoped<IUserService>(sp =>
                new UserServiceEmailDecorator(
                    new UserServiceAzureDecorator(
                        new UserServiceBlobDecorator(
                            new UserService(
                                sp.GetService<ApplicationContext>(),
                                sp.GetService<ILogger<UserService>>(),
                                sp.GetService<IMapper>(),
                                sp.GetService<IOptions<MemoryCacheTimeStamp>>(),
                                sp.GetService<IDistributedCache>()),
                            sp.GetService<IBlobServicesFacade>()),
                        sp.GetService<IGraphAPIService>()),
                    sp.GetService<IEmailNotificationService>(),
                    sp.GetService<IEmailService>(),
                    sp.GetService<IOptions<DeleteUserEmailConfig>>()));

            services.AddScoped<IUserProfileService>(sp => new UserProfileServiceBlobDecorator(
                new UserProfileService(
                    sp.GetService<ApplicationContext>(),
                    sp.GetService<ILogger<UserProfileService>>(),
                    sp.GetService<IMapper>(),
                    sp.GetService<IDistributedCache>()),
                sp.GetService<IBlobServicesFacade>()));

            services.AddScoped<ICompanyService>(sp => new CompanyServiceBlobDecorator(
                new CompanyServiceAzureDecorator(
                    new CompanyService(
                        sp.GetService<ApplicationContext>(),
                        sp.GetService<ILogger<CompanyService>>(),
                        sp.GetService<IMapper>(),
                        sp.GetService<IOptions<MemoryCacheTimeStamp>>(),
                        sp.GetService<IDistributedCache>()),
                    sp.GetService<IGraphAPIService>()),
                sp.GetService<IBlobServicesFacade>()));

            services.AddScoped<ISavedContentService, SavedContentService>();
            services.AddScoped<INotificationService, NotificationService>();
            services.AddScoped<IArticleService, ArticleService>();
            services.AddScoped<IForumService>(sp =>
            {
                var applicationContext = sp.GetRequiredService<ApplicationContext>();
                var logger = sp.GetRequiredService<ILogger<ForumService>>();
                var mapper = sp.GetRequiredService<IMapper>();
                var blobServicesFacade = sp.GetRequiredService<IBlobServicesFacade>();
                var commonServices = sp.GetRequiredService<ICommonService>();
                var forumService = new ForumService(applicationContext, logger, mapper, commonServices);
                return new ForumServiceBlobDecorator(forumService, blobServicesFacade);
            });
            services.AddScoped<IInitiativeService>(sp =>
            {
                var applicationContext = sp.GetRequiredService<ApplicationContext>();
                var logger = sp.GetRequiredService<ILogger<InitiativeService>>();
                var mapper = sp.GetRequiredService<IMapper>();
                var blobServicesFacade = sp.GetRequiredService<IBlobServicesFacade>();
                var commonService = sp.GetRequiredService<ICommonService>();
                var memoryCacheTimeStamp = sp.GetService<IOptions<MemoryCacheTimeStamp>>();
                var cache = sp.GetRequiredService<IDistributedCache>();
                var initiativeService = new InitiativeService(applicationContext, logger, mapper, commonService, memoryCacheTimeStamp, cache);
                return new InitiativeServiceBlobDecorator(initiativeService, blobServicesFacade);
            });
            services.AddScoped<IActivityService, ActivityService>();
            services.AddScoped<IUserPendingService>(sp =>
                new UserPendingEmailDecorator(
                    new UserPendingAzureDecorator(
                        new UserPendingService(
                            sp.GetRequiredService<ApplicationContext>(),
                            sp.GetService<ILogger<UserPendingService>>(),
                            sp.GetService<IMapper>()),
                        sp.GetService<IGraphAPIService>()),
                    sp.GetService<IEmailNotificationService>()));

            services.AddScoped<IAnnouncementService>(sp => new AnnouncementServiceBlobDecorator(
                new AnnouncementService(
                    sp.GetService<ApplicationContext>(),
                    sp.GetService<ILogger<AnnouncementService>>(),
                    sp.GetService<IMapper>(),
                    sp.GetService<IDistributedCache>()),
                sp.GetService<IBlobServicesFacade>()));

            services.AddScoped<IToolService>(sp => new ToolServiceBlobDecorator(
                sp.GetService<IBlobServicesFacade>(),
                new ToolServiceEmailDecorator(
                    new ToolService(
                    sp.GetService<ApplicationContext>(),
                    sp.GetService<ILogger<ToolService>>(),
                    sp.GetService<IMapper>()),
                sp.GetService<IUserService>(),
                sp.GetService<IEmailService>(),
                sp.GetService<IMapper>(),
                sp.GetService<IOptions<SolarQuoteEmailConfig>>())));

            services.AddScoped<IEventService, EventService>();
            services.AddScoped<IPublicDashboardService, PublicDashboardService>();
            services.AddScoped<IEventPublicService, EventService>();
            services.AddScoped<IEventService>(sp =>
            {
                var applicationContext = sp.GetRequiredService<ApplicationContext>();
                var logger = sp.GetRequiredService<ILogger<EventService>>();
                var mapper = sp.GetRequiredService<IMapper>();
                var commonServices = sp.GetRequiredService<ICommonService>();
                var blobServicesFacade = sp.GetRequiredService<IBlobServicesFacade>();
                var eventService = new EventService(applicationContext, logger, mapper, commonServices);
                return new EventServiceBlobDecorator(eventService, blobServicesFacade);
            });

            services.AddScoped<IProjectDetailsFactory, ProjectDetailsFactory>();
            services.AddScoped<IEmailAlertService, EmailAlertService>();

            services.AddScoped<ICommunityService>(sp => new CommunityServiceBlobDecorator(
                sp.GetService<IBlobServicesFacade>(),
                new CommunityService(
                    sp.GetService<ApplicationContext>(),
                    sp.GetRequiredService<ILogger<CommunityService>>(),
                    sp.GetService<IMapper>(),
                    sp.GetService<ICommonService>())
                    ));
            services.AddScoped<IFeedbackService>(sp =>
            {
                var applicationContext = sp.GetRequiredService<ApplicationContext>();
                var logger = sp.GetRequiredService<ILogger<FeedbackService>>();
                var mapper = sp.GetRequiredService<IMapper>();
                var blobServicesFacade = sp.GetRequiredService<IBlobServicesFacade>();
                var feedbackService = new FeedbackService(applicationContext, logger, mapper);
                return new FeedbackServiceBlobDecorator(feedbackService, blobServicesFacade);
            });
            // Infra Services
            services.AddScoped<IEmailNotificationService, EmailNotificationService>();
            services.AddScoped<IAzureSearchService, AzureSearchService>();

            // API Services
            services.AddScoped<IBlobApiService, BlobApiService>();
            services.AddScoped<IUserApiService, UserApiService>();
            services.AddScoped<IUserProfileApiService, UserProfileApiService>();
            services.AddScoped<IProjectApiService, ProjectApiService>();
            services.AddScoped<IConversationApiService, ConversationApiService>();
            services.AddScoped<IToolApiService, ToolApiService>();
            services.AddScoped<ICommonApiService, CommonApiService>();
            services.AddScoped<ICompanyApiService, CompanyApiService>();
            services.AddScoped<IScheduleDemoApiService, ScheduleDemoApiService>();
            services.AddScoped<IPublicDashboardApiService, PublicDashboardApiService>();
            services.AddScoped<ISavedContentApiService, SavedContentApiService>();
            services.AddScoped<INotificationApiService, NotificationApiService>();
            services.AddScoped<IArticleApiService, ArticleApiService>();
            services.AddScoped<IForumApiService, ForumApiService>();
            services.AddScoped<IInitiativeApiService, InitiativeApiService>();
            services.AddScoped<IActivityApiService, ActivityApiService>();
            services.AddScoped<IUserPendingApiService, UserPendingApiService>();
            services.AddScoped<IAnnouncementApiService, AnnouncementApiService>();
            services.AddScoped<IEventApiService, EventApiService>();
            services.AddScoped<ICommunityApiService, CommunityApiService>();
            services.AddScoped<ISearchApiService, SearchApiService>();
            services.AddScoped<IEmailAlertApiService, EmailAlertApiService>();
            services.AddScoped<IRecommenderSystemService, RecommenderSystemService>();
            services.AddScoped<IUnsubscribeEmailApiService, UnsubscribeEmailApiService>();
            services.AddScoped<IFeedbackApiService, FeedbackApiService>();

            // Validators
            services.AddScoped<IValidator<ProjectRequest>, ProjectRequestValidator>();

            services.AddScoped<IValidator<ProjectBatteryStorageDetailsRequest>, ProjectBatteryStorageDetailsRequestValidator>();
            services.AddScoped<IValidator<ProjectCarbonOffsetsDetailsRequest>, ProjectCarbonOffsetsDetailsRequestValidator>();
            services.AddScoped<IValidator<ProjectCommunitySolarDetailsRequest>, ProjectCommunitySolarDetailsRequestValidator>();
            services.AddScoped<IValidator<ProjectEACDetailsRequest>, ProjectEACDetailsRequestValidator>();
            services.AddScoped<IValidator<ProjectEfficiencyAuditsAndConsultingDetailsRequest>, ProjectEfficiencyAuditsAndConsultingDetailsRequestValidator>();
            services.AddScoped<IValidator<ProjectEfficiencyEquipmentMeasuresDetailsRequest>, ProjectEfficiencyEquipmentMeasuresDetailsRequestValidator>();
            services.AddScoped<IValidator<ProjectEmergingTechnologyDetailsRequest>, ProjectEmergingTechnologyDetailsRequestValidator>();
            services.AddScoped<IValidator<ProjectEVChargingDetailsRequest>, ProjectEVChargingDetailsRequestValidator>();
            services.AddScoped<IValidator<ProjectFuelCellsDetailsRequest>, ProjectFuelCellsDetailsRequestValidator>();
            services.AddScoped<IValidator<ProjectGreenTariffsDetailsRequest>, ProjectGreenTariffsDetailsRequestValidator>();
            services.AddScoped<IValidator<ProjectOffsitePowerPurchaseAgreementDetailsRequest>, ProjectOffsitePowerPurchaseAgreementDetailsValidator>();
            services.AddScoped<IValidator<ProjectOnsiteSolarDetailsRequest>, ProjectOnsiteSolarDetailsRequestValidator>();
            services.AddScoped<IValidator<ProjectRenewableRetailDetailsRequest>, ProjectRenewableRetailDetailsRequestValidator>();

            services.AddScoped<
                IValidator<BaseProjectDetailedRequest<ProjectBatteryStorageDetailsRequest>>,
                BaseProjectDetailedRequestValidator<ProjectBatteryStorageDetailsRequest>>();
            services.AddScoped<
                IValidator<BaseProjectDetailedRequest<ProjectCarbonOffsetsDetailsRequest>>,
                BaseProjectDetailedRequestValidator<ProjectCarbonOffsetsDetailsRequest>>();
            services.AddScoped<
                IValidator<BaseProjectDetailedRequest<ProjectCommunitySolarDetailsRequest>>,
                BaseProjectDetailedRequestValidator<ProjectCommunitySolarDetailsRequest>>();
            services.AddScoped<
                IValidator<BaseProjectDetailedRequest<ProjectEACDetailsRequest>>,
                BaseProjectDetailedRequestValidator<ProjectEACDetailsRequest>>();
            services.AddScoped<
                IValidator<BaseProjectDetailedRequest<ProjectEfficiencyAuditsAndConsultingDetailsRequest>>,
                BaseProjectDetailedRequestValidator<ProjectEfficiencyAuditsAndConsultingDetailsRequest>>();
            services.AddScoped<
                IValidator<BaseProjectDetailedRequest<ProjectEfficiencyEquipmentMeasuresDetailsRequest>>,
                BaseProjectDetailedRequestValidator<ProjectEfficiencyEquipmentMeasuresDetailsRequest>>();
            services.AddScoped<
                IValidator<BaseProjectDetailedRequest<ProjectEmergingTechnologyDetailsRequest>>,
                BaseProjectDetailedRequestValidator<ProjectEmergingTechnologyDetailsRequest>>();
            services.AddScoped<
                IValidator<BaseProjectDetailedRequest<ProjectEVChargingDetailsRequest>>,
                BaseProjectDetailedRequestValidator<ProjectEVChargingDetailsRequest>>();
            services.AddScoped<
                IValidator<BaseProjectDetailedRequest<ProjectFuelCellsDetailsRequest>>,
                BaseProjectDetailedRequestValidator<ProjectFuelCellsDetailsRequest>>();
            services.AddScoped<
                IValidator<BaseProjectDetailedRequest<ProjectGreenTariffsDetailsRequest>>,
                BaseProjectDetailedRequestValidator<ProjectGreenTariffsDetailsRequest>>();
            services.AddScoped<
                IValidator<BaseProjectDetailedRequest<ProjectOffsitePowerPurchaseAgreementDetailsRequest>>,
                BaseProjectDetailedRequestValidator<ProjectOffsitePowerPurchaseAgreementDetailsRequest>>();
            services.AddScoped<
                IValidator<BaseProjectDetailedRequest<ProjectOnsiteSolarDetailsRequest>>,
                BaseProjectDetailedRequestValidator<ProjectOnsiteSolarDetailsRequest>>();
            services.AddScoped<
                IValidator<BaseProjectDetailedRequest<ProjectRenewableRetailDetailsRequest>>,
                BaseProjectDetailedRequestValidator<ProjectRenewableRetailDetailsRequest>>();

            return services;
        }
    }
}
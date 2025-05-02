using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SE.Neo.Common.Enums;
using SE.Neo.Common.Extensions;
using SE.Neo.Core.Entities;
using SE.Neo.Core.Entities.CMS;
using SE.Neo.Core.Entities.ProjectDataTypes;
using SE.Neo.Core.Entities.ProjectDetails;
using SE.Neo.Core.Entities.TrackingActivity;
using SE.Neo.Core.Enums;
using SE.Neo.Core.Services.Interfaces;
using File = SE.Neo.Core.Entities.File;
using InitiativeScale = SE.Neo.Core.Entities.InitiativeScale;
using InitiativeStatus = SE.Neo.Core.Entities.InitiativeStatus;
using TimeZone = SE.Neo.Core.Entities.TimeZone;

namespace SE.Neo.Core.Data
{
    public class ApplicationContext : DbContext
    {
        private readonly ICurrentUserResolverService _currentUserResolverService;

        public ApplicationContext(
            DbContextOptions<ApplicationContext> options,
            ICurrentUserResolverService currentUserResolverService,
            IConfiguration configuration) : base(options)
        {
            _currentUserResolverService = currentUserResolverService;
        }

        // Project Catalog
        public DbSet<Project> Projects { get; set; }

        public DbSet<ProjectBatteryStorageDetails> ProjectsBatteryStorageDetails { get; set; }
        public DbSet<ProjectFuelCellsDetails> ProjectsFuelCellsDetails { get; set; }
        public DbSet<ProjectCarbonOffsetsDetails> ProjectsCarbonOffsetsDetails { get; set; }
        public DbSet<ProjectCommunitySolarDetails> ProjectsCommunitySolarDetails { get; set; }
        public DbSet<ProjectEACDetails> ProjectsEACDetails { get; set; }
        public DbSet<ProjectEfficiencyAuditsAndConsultingDetails> ProjectsEfficiencyAuditsAndConsultingDetails { get; set; }
        public DbSet<ProjectEfficiencyEquipmentMeasuresDetails> ProjectsEfficiencyEquipmentMeasuresDetails { get; set; }
        public DbSet<ProjectEmergingTechnologyDetails> ProjectsEmergingTechnologyDetails { get; set; }
        public DbSet<ProjectEVChargingDetails> ProjectsEVChargingDetails { get; set; }
        public DbSet<ProjectGreenTariffsDetails> ProjectsGreenTariffsDetails { get; set; }
        public DbSet<ProjectOffsitePowerPurchaseAgreementDetails> ProjectsOffsitePowerPurchaseAgreementDetails { get; set; }
        public DbSet<ProjectOnsiteSolarDetails> ProjectsOnsiteSolarDetails { get; set; }
        public DbSet<ProjectRenewableRetailDetails> ProjectsRenewableRetailDetails { get; set; }
        public DbSet<Entities.ProjectStatus> ProjectStatuses { get; set; }
        public DbSet<ProjectView> ProjectViews { get; set; }
        public DbSet<Company> Companies { get; set; }
        public DbSet<ProjectRegion> ProjectRegions { get; set; }
        public DbSet<ProjectTechnology> ProjectTechnologies { get; set; }
        public DbSet<Resource> Resource { get; set; }
        public DbSet<Entities.ResourceType> ResourceType { get; set; }
        public DbSet<ResourceTechnology> ResourceTechnology { get; set; }
        public DbSet<ResourceCategory> ResourceCategory { get; set; }
        public DbSet<ProjectContractStructure> ProjectContractStructures { get; set; }
        public DbSet<ProjectValueProvided> ProjectValuesProvided { get; set; }
        public DbSet<ProjectOffsitePPADetailsValueProvided> ProjectOffsitePPADetailsValuesProvided { get; set; }
        public DbSet<ProjectRenewableRetailDetailsPurchaseOption> ProjectRenewableRetailDetailsPurchaseOptions { get; set; }
        public DbSet<ProjectCarbonOffsetsDetailsTermLength> ProjectCarbonOffsetsDetailsTermLengths { get; set; }
        public DbSet<ProjectEACDetailsTermLength> ProjectEACDetailsTermLengths { get; set; }
        public DbSet<CompanyCategory> CompanyCategories { get; set; }
        public DbSet<Industry> Industries { get; set; }
        public DbSet<CompanyFollower> CompanyFollowers { get; set; }
        public DbSet<OffsitePPA> OffsitePPAs { get; set; }
        public DbSet<CompanyOffsitePPA> CompanyOffsitePPAs { get; set; }
        public DbSet<CompanyUrlLink> CompanyUrlLinks { get; set; }
        public DbSet<ContractStructure> ContractStructures { get; set; }
        public DbSet<ValueProvided> ValuesProvided { get; set; }
        public DbSet<IsoRto> IsoRto { get; set; }
        public DbSet<Entities.ProjectDataTypes.ProductType> ProductTypes { get; set; }
        public DbSet<Entities.ProjectDataTypes.SettlementType> SettlementTypes { get; set; }
        public DbSet<SettlementHubOrLoadZone> SettlementHubsOrLoadZones { get; set; }
        public DbSet<Entities.Currency> Currencies { get; set; }
        public DbSet<PricingStructure> PricingStructures { get; set; }
        public DbSet<EAC> EACs { get; set; }
        public DbSet<SettlementCalculationInterval> SettlementCalculationIntervals { get; set; }
        public DbSet<SettlementPriceInterval> SettlementPriceIntervals { get; set; }
        public DbSet<TermLength> TermLengths { get; set; }
        public DbSet<PurchaseOption> PurchaseOptions { get; set; }

        // Messaging & Forum
        public DbSet<Discussion> Discussions { get; set; }
        public DbSet<DiscussionUser> DiscussionUsers { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<DiscussionCategory> DiscussionCategories { get; set; }
        public DbSet<DiscussionRegion> DiscussionRegions { get; set; }
        public DbSet<DiscussionFollower> DiscussionFollowers { get; set; }
        public DbSet<DiscussionSource> DiscussionSources { get; set; }
        public DbSet<MessageLike> MessageLikes { get; set; }

        //Time Zone
        public DbSet<TimeZone> TimeZones { get; set; }
        public DbSet<Blob> Blobs { get; set; }
        public DbSet<BlobContainer> BlobContainers { get; set; }

        // Notifications
        public DbSet<UserNotification> UserNotifications { get; set; }

        // User Management
        public DbSet<User> Users { get; set; }
        public DbSet<UserRole> UserRoles { get; set; }
        public DbSet<UserPermission> UserPermissions { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<UserFollower> UserFollowers { get; set; }
        public DbSet<Entities.UserStatus> UserStatuses { get; set; }
        public DbSet<Entities.HeardVia> HeardVia { get; set; }

        // User Profile
        public DbSet<UserProfile> UserProfiles { get; set; }
        public DbSet<UserProfileUrlLink> UserProfileUrlLinks { get; set; }

        // User Pending (Admit Users)
        public DbSet<UserPending> UserPendings { get; set; }

        // CMS
        public DbSet<Article> Articles { get; set; }
        public DbSet<ArticleView> ArticleViews { get; set; }
        public DbSet<ArticleCategory> ArticleCategories { get; set; }
        public DbSet<ArticleSolution> ArticleSolutions { get; set; }
        public DbSet<ArticleTechnology> ArticleTechnologies { get; set; }
        public DbSet<ArticleRegion> ArticleRegions { get; set; }
        public DbSet<ArticleRole> ArticleRoles { get; set; }
        public DbSet<ArticleContentTag> ArticleContentTags { get; set; }
        public DbSet<ContentTag> ContentTags { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Solution> Solutions { get; set; }
        public DbSet<Technology> Technologies { get; set; }
        public DbSet<Region> Regions { get; set; }
        public DbSet<UserProfileCategory> UserProfileCategories { get; set; }
        public DbSet<UserProfileRegion> UserProfileRegions { get; set; }

        // Saved
        public DbSet<ProjectSaved> SavedProjects { get; set; }
        public DbSet<DiscussionSaved> SavedForums { get; set; }
        public DbSet<ArticleSaved> SavedArticles { get; set; }

        //Tools
        public DbSet<Tool> Tools { get; set; }
        public DbSet<ToolPinned> ToolsPinned { get; set; }
        public DbSet<ToolRole> ToolRoles { get; set; }
        public DbSet<ToolCompany> ToolCompanies { get; set; }
        public DbSet<SolarQuote> SolarQuotes { get; set; }
        public DbSet<SolarQuoteContractStructure> SolarQuoteContractStructures { get; set; }
        public DbSet<SolarQuoteValueProvided> SolarQuoteValuesProvided { get; set; }

        // Tracking
        public DbSet<Activity> Activities { get; set; }
        public DbSet<PublicSiteActivity> PublicSiteActivities { get; set; }
        public DbSet<Entities.TrackingActivity.ActivityType> ActivityTypes { get; set; }
        public DbSet<State> States { get; set; }
        public DbSet<Country> Countries { get; set; }

        // Announcements
        public DbSet<Announcement> Announcements { get; set; }

        // Events
        public DbSet<Event> Events { get; set; }
        public DbSet<EventAttendee> EventAttendees { get; set; }
        public DbSet<EventCategory> EventCategories { get; set; }
        public DbSet<EventOccurrence> EventOccurrences { get; set; }
        public DbSet<EventLink> EventLinks { get; set; }
        public DbSet<EventModerator> EventModerators { get; set; }
        public DbSet<EventInvitedRole> EventInvitedRoles { get; set; }
        public DbSet<EventInvitedCategory> EventInvitedCategories { get; set; }
        public DbSet<EventInvitedRegion> EventInvitedRegions { get; set; }
        public DbSet<EventInvitedUser> EventInvitedUsers { get; set; }

        // Email Alerts
        public DbSet<EmailAlert> EmailAlerts { get; set; }
        public DbSet<UserEmailAlert> UserEmailAlerts { get; set; }

        public DbSet<JobLog> JobLogs { get; set; }
        public DbSet<CompanyDomain> CompanyDomains { get; set; }
        public DbSet<DiscoverabilityProjectsData> DiscoverabilityProjectsData { get; set; }


        //Feedback
        public DbSet<Feedback> Feedbacks { get; set; }



        //Feedback
        public DbSet<CompanyAnnouncement> CompanyAnnouncement { get; set; }
        public DbSet<CompanyAnnouncementRegion> CompanyAnnouncementRegion { get; set; }


        //Initative
        public DbSet<Initiative> Initiative { get; set; }
        //public DbSet<InitiativeActivity> InitiativeActivity { get; set; }
        public DbSet<InitiativeArticle> InitiativeArticle { get; set; }
        public DbSet<InitiativeConversation> InitiativeConversation { get; set; }
        public DbSet<InitiativeCommunity> InitiativeCommunity { get; set; }
        public DbSet<InitiativeProject> InitiativeProject { get; set; }
        public DbSet<InitiativeRegion> InitiativeRegions { get; set; }
        public DbSet<InitiativeScale> InitiativeScale { get; set; }
        public DbSet<InitiativeStatus> InitiativeStatus { get; set; }
        public DbSet<InitiativeTool> InitiativeTool { get; set; }
        public DbSet<InitiativeStep> InitiativeSteps { get; set; }
        public DbSet<InitiativeSubStep> InitiativeSubSteps { get; set; }
        public DbSet<InitiativeProgressDetails> InitiativeProgressDetails { get; set; }
        public DbSet<InitiativeRecommendationActivity> InitiativeRecommendationActivity { get; set; }
        public DbSet<InitiativeFile> InitiativeFile { get; set; }

        public DbSet<CompanyFile> CompanyFile { get; set; }
        public DbSet<File> File { get; set; }
        public DbSet<InitiativeCollaborator> InitiativeCollaborator { get; set; }
        public DbSet<Skills> Skills { get; set; }
        public DbSet<UserSkillsByCategory> UserSkillsByCategory { get; set; }
        public DbSet<SkillsByCategory> SkillsByCategory { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            foreach (var relationship in modelBuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            modelBuilder.Entity<Category>()
                .Property(b => b.IsDeleted)
                .HasDefaultValue(0);

            modelBuilder.Entity<Solution>()
                  .Property(b => b.IsDeleted)
                  .HasDefaultValue(0);

            modelBuilder.Entity<Technology>()
                  .Property(b => b.IsDeleted)
                  .HasDefaultValue(0);

            modelBuilder.Entity<Region>()
                  .Property(b => b.IsDeleted)
                  .HasDefaultValue(0);

            modelBuilder.Entity<Entities.ProjectStatus>().HasData(
                    Enum.GetValues(typeof(Enums.ProjectStatus))
                        .Cast<Enums.ProjectStatus>()
                        .Select(e => new Entities.ProjectStatus()
                        {
                            Id = e,
                            Name = e.ToString()
                        })
                );

            modelBuilder.Entity<DiscussionSource>().HasData(
                Enum.GetValues(typeof(DiscussionSourceType))
                    .Cast<DiscussionSourceType>()
                    .Select(e => new DiscussionSource()
                    {
                        Id = e,
                        Name = e.GetDescription()
                    })
            );

            modelBuilder.Entity<Entities.ResourceType>().HasData(
                    Enum.GetValues(typeof(Enums.ResourceType))
                        .Cast<Enums.ResourceType>()
                        .Select(e => new Entities.ResourceType()
                        {
                            Id = e,
                            Name = e.ToString()
                        })
                );

            modelBuilder.Entity<ContractStructure>().HasData(
                    Enum.GetValues(typeof(ContractStructureType))
                        .Cast<ContractStructureType>()
                        .Select(e => new ContractStructure()
                        {
                            Id = e,
                            Name = e.GetDescription()
                        })
                );

            modelBuilder.Entity<ValueProvided>().HasData(
                    Enum.GetValues(typeof(ValueProvidedType))
                        .Cast<ValueProvidedType>()
                        .Select(e => new ValueProvided()
                        {
                            Id = e,
                            Name = e.GetDescription()
                        })
                );


            modelBuilder.Entity<IsoRto>().HasData(
                    Enum.GetValues(typeof(IsoRtoType))
                        .Cast<IsoRtoType>()
                        .Select(e => new IsoRto()
                        {
                            Id = e,
                            Name = e.ToString()
                        })
                );

            modelBuilder.Entity<Entities.ProjectDataTypes.ProductType>().HasData(
                    Enum.GetValues(typeof(Enums.ProductType))
                        .Cast<Enums.ProductType>()
                        .Select(e => new Entities.ProjectDataTypes.ProductType()
                        {
                            Id = e,
                            Name = e.GetDescription()
                        })
                );

            modelBuilder.Entity<Entities.ProjectDataTypes.SettlementType>().HasData(
                    Enum.GetValues(typeof(Enums.SettlementType))
                        .Cast<Enums.SettlementType>()
                        .Select(e => new Entities.ProjectDataTypes.SettlementType()
                        {
                            Id = e,
                            Name = e.ToString()
                        })
                );

            modelBuilder.Entity<SettlementHubOrLoadZone>().HasData(
                    Enum.GetValues(typeof(SettlementHubOrLoadZoneType))
                        .Cast<SettlementHubOrLoadZoneType>()
                        .Select(e => new SettlementHubOrLoadZone()
                        {
                            Id = e,
                            Name = e.GetDescription()
                        })
                );

            modelBuilder.Entity<Entities.Currency>().HasData(
                    Enum.GetValues(typeof(Enums.Currency))
                        .Cast<Enums.Currency>()
                        .Select(e => new Entities.Currency()
                        {
                            Id = e,
                            Name = e.ToString()
                        })
                );

            modelBuilder.Entity<PricingStructure>().HasData(
                    Enum.GetValues(typeof(PricingStructureType))
                        .Cast<PricingStructureType>()
                        .Select(e => new PricingStructure()
                        {
                            Id = e,
                            Name = e.GetDescription()
                        })
                );

            modelBuilder.Entity<EAC>().HasData(
                    Enum.GetValues(typeof(EACType))
                        .Cast<EACType>()
                        .Select(e => new EAC()
                        {
                            Id = e,
                            Name = e.GetDescription()
                        })
                );


            modelBuilder.Entity<SettlementCalculationInterval>().HasData(
                    Enum.GetValues(typeof(SettlementCalculationIntervalType))
                        .Cast<SettlementCalculationIntervalType>()
                        .Select(e => new SettlementCalculationInterval()
                        {
                            Id = e,
                            Name = e.GetDescription()
                        })
                );

            modelBuilder.Entity<SettlementPriceInterval>().HasData(
                    Enum.GetValues(typeof(SettlementPriceIntervalType))
                        .Cast<SettlementPriceIntervalType>()
                        .Select(e => new SettlementPriceInterval()
                        {
                            Id = e,
                            Name = e.GetDescription()
                        })
                );

            modelBuilder.Entity<TermLength>().HasData(
                    Enum.GetValues(typeof(TermLengthType))
                        .Cast<TermLengthType>()
                        .Select(e => new TermLength()
                        {
                            Id = e,
                            Name = e.GetDescription()
                        })
                );

            modelBuilder.Entity<PurchaseOption>().HasData(
                Enum.GetValues(typeof(PurchaseOptionType))
                    .Cast<PurchaseOptionType>()
                    .Select(e => new PurchaseOption()
                    {
                        Id = e,
                        Name = e.GetDescription()
                    })
            );

            modelBuilder.Entity<Entities.UserStatus>().HasData(
                    Enum.GetValues(typeof(Enums.UserStatus))
                        .Cast<Enums.UserStatus>()
                        .Select(e => new Entities.UserStatus()
                        {
                            Id = e,
                            Name = e.ToString()
                        })
                );

            modelBuilder.Entity<Entities.HeardVia>().HasData(
                    Enum.GetValues(typeof(Enums.HeardVia))
                        .Cast<Enums.HeardVia>()
                        .Select(p => new Entities.HeardVia
                        {
                            Id = p,
                            Name = p.GetDescription()
                        })
                );

            modelBuilder.Entity<Permission>().HasData(
                    Enum.GetValues(typeof(PermissionType))
                        .Cast<PermissionType>()
                        .Select(p => new Permission
                        {
                            Id = p,
                            Name = p.GetDescription()
                        })
                );

            modelBuilder.Entity<BlobContainer>().HasData(
                    Enum.GetValues(typeof(BlobType))
                        .Cast<BlobType>()
                        .Select(bcn => new BlobContainer
                        {
                            Id = bcn,
                            Name = bcn.ToString()
                        })
                );

            modelBuilder.Entity<Entities.TrackingActivity.ActivityType>().HasData(
                    Enum.GetValues(typeof(Enums.ActivityType))
                        .Cast<Enums.ActivityType>()
                        .Select(a => new Entities.TrackingActivity.ActivityType
                        {
                            Id = a,
                            Name = a.GetDescription()
                        })
                );

            modelBuilder.Entity<Entities.TrackingActivity.ActivityLocation>().HasData(
                    Enum.GetValues(typeof(Enums.ActivityLocation))
                        .Cast<Enums.ActivityLocation>()
                        .Select(a => new Entities.TrackingActivity.ActivityLocation
                        {
                            Id = a,
                            Name = a.GetDescription()
                        })
                );

            modelBuilder.Entity<User>()
                  .Property(b => b.HeardViaId)
                  .HasDefaultValue(Enums.HeardVia.Other);

            modelBuilder.Entity<Entities.CompanyStatus>().HasData(
                    Enum.GetValues(typeof(Enums.CompanyStatus))
                        .Cast<Enums.CompanyStatus>()
                        .Select(e => new Entities.CompanyStatus()
                        {
                            Id = e,
                            Name = e.ToString()
                        })
                );

            modelBuilder.Entity<OffsitePPA>().HasData(
                    Enum.GetValues(typeof(OffsitePPAs))
                        .Cast<OffsitePPAs>()
                        .Select(p => new OffsitePPA
                        {
                            Id = p,
                            Name = p.GetDescription()
                        })
                );

            modelBuilder.Entity<Entities.CMS.ArticleType>().HasData(
                        Enum.GetValues(typeof(Enums.ArticleType))
                        .Cast<Enums.ArticleType>()
                        .Select(i => new Entities.CMS.ArticleType
                        {
                            Id = i,
                            Name = i.GetDescription()
                        })
               );

            modelBuilder.Entity<Entities.Responsibility>().HasData(
                     Enum.GetValues(typeof(Enums.Responsibility))
                         .Cast<Enums.Responsibility>()
                         .Select(p => new Entities.Responsibility
                         {
                             Id = p,
                             Name = p.ToString()
                         })
                 );

            modelBuilder.Entity<Entities.TrackingActivity.DashboardClickElementActionType>().HasData(
                 Enum.GetValues(typeof(Enums.DashboardClickElementActionType))
                     .Cast<Enums.DashboardClickElementActionType>()
                     .Select(p => new Entities.TrackingActivity.DashboardClickElementActionType
                     {
                         Id = p,
                         Name = p.GetDescription()
                     })
             );

            modelBuilder.Entity<Entities.TrackingActivity.DashboardResourceViewAllType>().HasData(
                 Enum.GetValues(typeof(Enums.DashboardResourceViewAllType))
                     .Cast<Enums.DashboardResourceViewAllType>()
                     .Select(p => new Entities.TrackingActivity.DashboardResourceViewAllType
                     {
                         Id = p,
                         Name = p.GetDescription()
                     })
             );

            modelBuilder.Entity<Entities.TrackingActivity.NavMenuItem>().HasData(
                 Enum.GetValues(typeof(Enums.NavMenuItem))
                     .Cast<Enums.NavMenuItem>()
                     .Select(p => new Entities.TrackingActivity.NavMenuItem
                     {
                         Id = p,
                         Name = p.GetDescription()
                     })
             );

            modelBuilder.Entity<UserFollower>()
                .HasOne(u => u.Follower)
                .WithMany(u => u.FollowedUsers)
                .HasForeignKey(u => u.FollowerId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<UserFollower>()
                .HasOne(u => u.Followed)
                .WithMany(u => u.FollowerUsers)
                .HasForeignKey(u => u.FollowedId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Company>()
                .Property(b => b.MDMKey)
                .HasDefaultValue(string.Empty);

            modelBuilder.Entity<Country>()
                .Property(b => b.Code)
                .HasDefaultValue(string.Empty);

            modelBuilder.Entity<Country>()
                .Property(b => b.Code3)
                .HasDefaultValue(string.Empty);

            modelBuilder.Entity<Article>()
                .Property(b => b.PdfUrl)
                .HasDefaultValue(string.Empty);

            base.OnModelCreating(modelBuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string environment = Environment.GetEnvironmentVariable("APP_ENV") ?? "dev";
            if (environment.GetValueFromDescription<EnvironmentEnum>() != EnvironmentEnum.Prod &&
                environment.GetValueFromDescription<EnvironmentEnum>() != EnvironmentEnum.PreProd)
            {
                optionsBuilder = optionsBuilder
                    .LogTo(Console.WriteLine, LogLevel.Information, DbContextLoggerOptions.DefaultWithUtcTime)
                    .EnableDetailedErrors();
            }
            optionsBuilder.AddInterceptors(new TimeStampDbInterceptor(_currentUserResolverService));
        }
    }
}
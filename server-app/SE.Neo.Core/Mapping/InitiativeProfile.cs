using AutoMapper;
using SE.Neo.Common.Enums;
using SE.Neo.Common.Models.Conversation;
using SE.Neo.Common.Models.Initiative;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Common.Models.User;
using SE.Neo.Core.Entities;

namespace SE.Neo.Core.Mapping
{
    public class InitiativeProfile : Profile
    {
        public InitiativeProfile()
        {
            CreateMap<Initiative, InitiativeDTO>();
            CreateMap<Initiative, InitiativeAdminDTO>()
                .ForMember(dest => dest.Category, opts => opts.MapFrom(src => src.ProjectType))
                .ForMember(dest => dest.StatusId, opts => opts.MapFrom(src => (int)src.StatusId))
                .ForMember(dest => dest.StatusName, opts => opts.MapFrom(src => src.StatusId.ToString()))
                .ForMember(dest => dest.Phase, opts => opts.MapFrom(src => src.InitiativeStep.Name))
                .ForMember(dest => dest.Title, opts => opts.MapFrom(src => src.Title))
                .ForMember(dest => dest.User, opts => opts.MapFrom(src => src.User))
                .ForMember(dest => dest.ModifiedOn, opts => opts.MapFrom(src => src.ModifiedOn))
                .ForMember(dest => dest.Company, opts => opts.MapFrom(src => src.Company))
                .ForMember(dest => dest.Regions, opts => opts.MapFrom(src => src.Regions.Select(o => o.Region)));

            CreateMap<InitiativeDTO, Initiative>().ForMember(dest => dest.UserId, opts => opts.MapFrom(src => src.User.Id))
                   .ForMember(dest => dest.CompanyId, opts => opts.MapFrom(src => src.User.CompanyId))
                   .ForMember(dest => dest.User, opt => opt.Ignore());

            CreateMap<Initiative, InitiativeDTO>()
                   .ForMember(dest => dest.Regions, opt => opt.Ignore());
            CreateMap<InitiativeArticle, ArticleForInitiativeDTO>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Article.Id))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Article.Title))
                .ForMember(dest => dest.TypeId, opts => opts.MapFrom(src => src.Article.TypeId))
                .ForMember(dest => dest.ImageUrl, opts => opts.MapFrom(src => src.Article.ImageUrl))
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.Article.ArticleCategories.Where(x => x.CategoryId != null).Select(x => x.Category)));
            CreateMap<InitiativeStep, InitiativeStepDTO>()
           .ForMember(dest => dest.StepId, opt => opt.MapFrom(src => src.Id))
           .ForMember(dest => dest.SubSteps, opt => opt.MapFrom(src => src.InitiativeSubSteps.Select(x => x)));
            CreateMap<InitiativeProgressDetails, InitiativeSubStepProgressDTO>()
                    .ForMember(dest => dest.InitiativeId, opt => opt.Ignore())
                  .ForMember(dest => dest.CurrentStep, opt => opt.Ignore());
            CreateMap<InitiativeSubStepDTO, InitiativeSubStep>();
            CreateMap<InitiativeSubStep, InitiativeSubStepDTO>()
                .ForMember(dest => dest.SubStepId, opt => opt.MapFrom(src => src.Id));
            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();

            CreateMap<Discussion, ConversationForInitiativeDTO>();


            CreateMap<DiscussionUser, ConversationUserForInitiativeDTO>()
            .ForMember(dest => dest.Name, opts => opts.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}"))
            .ForMember(dest => dest.Company, opts => opts.MapFrom(src => src.User.Company.Name))
             .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.User.Id))
             .ForMember(dest => dest.ImageName, opts => opts.MapFrom(src => src.User.ImageName))
             .ForMember(dest => dest.Image, opts => opts.MapFrom(src => src.User.Image))
             .ForMember(dest => dest.StatusId, opts => opts.MapFrom(src => src.User.StatusId));

            CreateMap<InitiativeCollaborator, UserDTO>()
            .ForMember(dest => dest.Username, opts => opts.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}"))
            .ForMember(dest => dest.Company, opts => opts.MapFrom(src => src.User.Company.Name))
             .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.User.Id))
             .ForMember(dest => dest.ImageName, opts => opts.MapFrom(src => src.User.ImageName))
             .ForMember(dest => dest.Image, opts => opts.MapFrom(src => src.User.Image))
             .ForMember(dest => dest.StatusId, opts => opts.MapFrom(src => src.User.StatusId));

            CreateMap<InitiativeTool, ToolForInitiativeDTO>()
                .ForMember(dest => dest.Title, opts => opts.MapFrom(src => src.Tool.Title))
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.Tool.Id))
                .ForMember(dest => dest.Description, opts => opts.MapFrom(src => src.Tool.Description))
                .ForMember(dest => dest.ImageUrl, opts => opts.MapFrom(src => src.Tool.Icon));

            CreateMap<InitiativeProject, ProjectForInitiativeDTO>()
                .ForMember(dest => dest.Title, opts => opts.MapFrom(src => src.Project.Title))
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.ProjectId))
                .ForMember(dest => dest.SubTitle, opts => opts.MapFrom(src => src.Project.SubTitle))
                .ForMember(dest => dest.Company, opts => opts.MapFrom(src => src.Project.Company))
                .ForMember(dest => dest.Regions, opts => opts.MapFrom(src => src.Project.Regions.Select(o => o.Region)))
                .ForMember(dest => dest.Category, opts => opts.MapFrom(src => src.Project.Category));

            CreateMap<InitiativeFileDTO, Entities.File>();


            CreateMap<InitiativeConversation, ConversationMessageDTO>();


            CreateMap<UserProfileCategory, CommunityUserForInitiativeDTO>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.UserProfile.User.Id))
               .ForMember(dest => dest.TypeId, opts => opts.MapFrom(src => CommunityItemType.User));

            CreateMap<InitiativeCommunity, CommunityUserForInitiativeDTO>()
               .ForMember(dest => dest.TypeId, opts => opts.MapFrom(src => CommunityItemType.User))
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.UserId))
                .ForMember(dest => dest.FirstName, opts => opts.MapFrom(src => src.User.FirstName))
                .ForMember(dest => dest.LastName, opts => opts.MapFrom(src => src.User.LastName))
                .ForMember(dest => dest.CompanyName, opts => opts.MapFrom(src => src.User.Company.Name))
                .ForMember(dest => dest.Roles, opts => opts.MapFrom(src => src.User.Roles.Where(x => x.RoleId != (int) RoleType.All).Select(x => x.Role)))
                .ForMember(dest => dest.ImageName, opts => opts.MapFrom(src => src.User.ImageName))
                .ForMember(dest => dest.Image, opts => opts.MapFrom(src => src.User.Image))
               .ForMember(dest => dest.JobTitle, opts => opts.MapFrom(src => src.User.UserProfile.JobTitle))
               .ForMember(dest => dest.Categories, opts => opts.MapFrom(src => src.User.UserProfile.Categories.Take(1).Select(x => x.Category).ToList()))
               .ForMember(dest => dest.TagsTotalCount, opts => opts.MapFrom(src => src.User.UserProfile.Categories.Count + src.User.UserProfile.Regions.Count));
        }
    }
}

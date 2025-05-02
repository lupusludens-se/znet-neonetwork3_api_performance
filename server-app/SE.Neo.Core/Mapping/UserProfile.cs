using AutoMapper;
using SE.Neo.Common.Extensions;
using SE.Neo.Common.Models.User;
using SE.Neo.Common.Models.UserProfile;
using SE.Neo.Core.Entities;

namespace SE.Neo.Core.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDTO>()
                .ForMember(dest => dest.Roles, opts => opts.MapFrom(src => src.Roles.Select(o => o.Role)))
                .ForMember(dest => dest.Permissions, opts => opts.MapFrom(src => src.Permissions.Select(o => o.Permission)))
                .ForMember(dest => dest.UserProfile, opts => opts.MapFrom(src => src.UserProfile))
                .ForMember(dest => dest.StatusName, opts => opts.MapFrom(src => src.StatusId.ToString()))
                .ForMember(dest => dest.ApprovedBy, opts => opts.MapFrom(src => src.CreatedByUserId != null ? "Admin" : "System"))
                .ForMember(dest => dest.HeardViaName, opts => opts.MapFrom(src => src.HeardViaId.GetDescription()));

            CreateMap<UserDTO, User>()
                .ForMember(m => m.TimeZone, i => i.Ignore())
                .ForMember(m => m.Company, i => i.Ignore())
                .ForMember(m => m.Roles, i => i.Ignore())
                .ForMember(m => m.Permissions, i => i.Ignore())
                .ForMember(m => m.Country, i => i.Ignore())
                .ForMember(m => m.UserProfile, i => i.Ignore())
                .ForMember(m => m.AzureId, i => i.Condition(u => !string.IsNullOrEmpty(u.AzureId)));

            CreateMap<Entities.UserProfile, UserProfileDTO>()
                .ForMember(dest => dest.Categories, opts => opts.MapFrom(src => src.Categories.Select(o => o.Category)))
                .ForMember(dest => dest.Regions, opts => opts.MapFrom(src => src.Regions.Select(o => o.Region)))
                .ForMember(dest => dest.ResponsibilityName, opts => opts.MapFrom(src => src.ResponsibilityId.ToString()));

            CreateMap<UserProfileUrlLink, UserProfileUrlLinkDTO>();

            CreateMap<UserProfileDTO, Entities.UserProfile>()
                .ForMember(m => m.Categories, i => i.Ignore())
                .ForMember(m => m.Regions, i => i.Ignore())
                .ForMember(m => m.User, i => i.Ignore())
                .ForMember(m => m.State, i => i.Ignore())
                .ForMember(m => m.UrlLinks, i => i.Ignore())
                .ForMember(m => m.SkillsByCategory, i => i.Ignore());


            CreateMap<UserPending, UserPendingDTO>()
                .ForMember(dest => dest.HeardViaName, opts => opts.MapFrom(src => src.HeardViaId.GetDescription()))
                .ForMember(dest => dest.CreatedDate, opts => opts.MapFrom(src => src.CreatedOn));

            CreateMap<UserPendingDTO, UserPending>()
                 .ForMember(m => m.CompanyName, i => i.Condition(src => !string.IsNullOrEmpty(src.CompanyName)))
                 .ForMember(m => m.TimeZoneId, i => i.Condition(src => src.TimeZoneId > 0))
                 .ForMember(m => m.Country, i => i.Ignore());

            CreateMap<UserPending, User>()
                .ForMember(dest => dest.Id, i => i.Ignore())
                .ForMember(dest => dest.StatusId, opts => opts.MapFrom(src => Enums.UserStatus.Onboard))
                .ForMember(dest => dest.Username, opts => opts.MapFrom(src => src.Email))
                .ForMember(dest => dest.CreatedOn, i => i.Ignore());

            CreateMap<UserFollower, UserFollowerDTO>()
                .ForMember(dest => dest.FirstName, opts => opts.MapFrom(src => src.Follower.FirstName))
                .ForMember(dest => dest.LastName, opts => opts.MapFrom(src => src.Follower.LastName))
                .ForMember(dest => dest.Email, opts => opts.MapFrom(src => src.Follower.Email))
                .ForMember(dest => dest.JobTitle, opts => opts.MapFrom(src => src.Follower.UserProfile.JobTitle))
                .ForMember(dest => dest.ImageName, opts => opts.MapFrom(src => src.Follower.ImageName))
                .ForMember(dest => dest.Company, opts => opts.MapFrom(src => src.Follower.Company.Name))
                .ForMember(dest => dest.Image, opts => opts.MapFrom(src => src.Follower.Image));

            CreateMap<UserSkillsByCategory, SkillsByCategoryDTO>().ReverseMap();

            CreateMap<Skills, SkillsByCategoryDTO>()
            .ForMember(dest => dest.SkillId, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.SkillName, opt => opt.MapFrom(src => src.Name)).ReverseMap();

            CreateMap<SkillsByCategory, SkillsByCategoryDTO>()
           .ForMember(dest => dest.SkillName, opt => opt.MapFrom(src => src.Skills.Name))
           .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name)).ReverseMap();            
        }
    }
}
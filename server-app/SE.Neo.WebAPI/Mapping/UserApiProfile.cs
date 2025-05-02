using AutoMapper;
using Microsoft.CodeAnalysis;
using SE.Neo.Common.Models.CMS;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Common.Models.User;
using SE.Neo.Common.Models.UserProfile;
using SE.Neo.Core.Entities;
using SE.Neo.WebAPI.Models.Shared;
using SE.Neo.WebAPI.Models.User;
using SE.Neo.WebAPI.Models.UserProfile;

namespace SE.Neo.WebAPI.Mapping
{
    public class UserApiProfile : Profile
    {
        public UserApiProfile()
        {
            CreateMap<UserDTO, UserResponse>();

            CreateMap<UserDTO, UserExportResponse>()
                .ForMember(dest => dest.Status, opts => opts.MapFrom(src => src.StatusName))
                .ForMember(dest => dest.Company, opts => opts.MapFrom(src => src.Company.Name))
                .ForMember(dest => dest.Country, opts => opts.MapFrom(src => src.Country.Name))
                .ForMember(dest => dest.State, opts => opts.MapFrom(src => src.UserProfile.State.Name))
                .ForMember(dest => dest.Regions, opts => opts.MapFrom(src => String.Join(", ", src.UserProfile.Regions.Select(s => s.Name).ToList())))
                .ForMember(dest => dest.Roles, opts => opts.MapFrom(src => String.Join(", ", src.Roles.Where(w => w.Id != (int)Common.Enums.RoleType.All).Select(s => s.Name).ToList())));

            CreateMap<UserProfileDTO, UserProfileResponse>();

            CreateMap<UserFollowerDTO, UserFollowerResponse>()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.FollowerId));


            CreateMap<UserProfileUrlLinkDTO, UrlLinkModel>();

            CreateMap<UrlLinkModel, UserProfileUrlLinkDTO>();

            CreateMap<UserRequest, UserDTO>(); 

            CreateMap<UserProfileRequest, UserProfileDTO>()
                .ForMember(dest => dest.About, opts => opts.NullSubstitute(string.Empty))
                .ForMember(dest => dest.LinkedInUrl, opts => opts.NullSubstitute(string.Empty));

            CreateMap<User, UserModel>()
                .ForMember(dest => dest.Status, opts => opts.MapFrom(src => src.StatusId))
                .ForMember(dest => dest.RoleIds, opts => opts.MapFrom(src => src.Roles.Select(r => r.RoleId)))
                .ForMember(dest => dest.CMSRoleIds, opts => opts.MapFrom(src => src.Roles.Where(w => w.Role.CMSRoleId.HasValue).Select(r => r.Role.CMSRoleId)))
                .ForMember(dest => dest.HasUserProfile, opts => opts.MapFrom(src => src.UserProfile != null))
                .ForMember(dest => dest.PermissionIds, opts => opts.MapFrom(src => src.Permissions
                                                                            .Select(r => r.PermissionId)
                                                                        .Union(src.Roles
                                                                            .SelectMany(r => r.Role.Permissions
                                                                                .Select(p => p.PermissionId)))
                                                                        .Distinct()));
            CreateMap<UserResponse, UserDTO>();

            CreateMap<UserProfileInterestRequest, TaxonomyDTO>();

            CreateMap<UserPendingAddRequest, UserPendingDTO>();

            CreateMap<UserPendingEditRequest, UserPendingDTO>();

            CreateMap<UserPendingDTO, UserPendingItemResponse>();

            CreateMap<UserPendingDTO, UserPendingListItemResponse>();

            CreateMap<SuggestionDTO, SuggestionModel>();

            CreateMap<UserProfileSuggestionDTO, UserProfileSuggestionResponse>();
            CreateMap< SkillsByCategoryModel, SkillsByCategoryDTO >();
            CreateMap<SkillsByCategoryDTO, SkillsByCategoryResponse>();
            CreateMap< SkillsByCategoryDTO, SkillsByCategoryModel>();
            CreateMap<UserSkillsByCategory, SkillsByCategoryDTO>()
            .ForMember(dest => dest.SkillName, opt => opt.MapFrom(src => src.Skills.Name))
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Categories.Name));
        }
    }
}
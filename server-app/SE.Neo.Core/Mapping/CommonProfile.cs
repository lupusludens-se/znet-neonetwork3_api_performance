using AutoMapper;
using SE.Neo.Common.Models.CMS;
using SE.Neo.Common.Models.Initiative;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Core.Entities;
using SE.Neo.Core.Entities.CMS;
using SE.Neo.Core.Entities.ProjectDataTypes;

namespace SE.Neo.Core.Mapping
{
    public class CommonProfile : Profile
    {
        public CommonProfile()
        {
            CreateMap<Role, RoleDTO>()
                .ForMember(dest => dest.Permissions, opts => opts.MapFrom(src => src.Permissions.Select(o => o.Permission)));

            CreateMap<Permission, PermissionDTO>()
                .ForMember(dest => dest.Roles, opts => opts.MapFrom(src => src.Roles.Select(o => o.Role)));

            CreateMap<Category, CategoryDTO>()
                .ForMember(dest => dest.Technologies, opts => opts.MapFrom(src => src.Technologies.Select(o => o.Technology)))
                .ForMember(dest => dest.Resources, opts => opts.MapFrom(src => src.CategoryResources.Select(o => o.Resource)));

            CreateMap<Solution, SolutionDTO>();

            CreateMap<Technology, TechnologyDTO>()
                .ForMember(dest => dest.Categories, opts => opts.MapFrom(src => src.Categories.Select(o => o.Category)))
                .ForMember(dest => dest.Resources, opts => opts.MapFrom(src => src.TechnologyResources.Select(o => o.Resource)));

            CreateMap<Resource, ResourceDTO>()
                .ForMember(dest => dest.Type, opts => opts.MapFrom(src => src.TypeId.ToString()))
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.ResourceCategories.Where(x => x.Category != null).Select(x => x.Category)))
                .ForMember(dest => dest.Technologies, opt => opt.MapFrom(src => src.ResourceTechnologies.Where(x => x.Technology != null).Select(x => x.Technology)));

            CreateMap<PurchaseOption, BaseIdNameDTO>();

            CreateMap<TermLength, BaseIdNameDTO>();

            CreateMap<ValueProvided, BaseIdNameDTO>();

            CreateMap<Region, RegionDTO>();

            CreateMap<ArticleDTO, Article>();

            CreateMap<Country, CountryDTO>();

            CreateMap<ArticleForInitiativeDTO, Article>();

            CreateMap<State, StateDTO>();

            CreateMap<RoleDTO, Role>()
                .ForMember(m => m.Permissions, i => i.Ignore())
                .ForMember(m => m.Users, i => i.Ignore());

            CreateMap<HeardVia, BaseIdNameDTO>();

            CreateMap<Entities.TimeZone, TimeZoneDTO>();
        }
    }
}
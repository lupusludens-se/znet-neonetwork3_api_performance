using AutoMapper;
using SE.Neo.Infrastructure.Models.AzureSearch;
using SE.Neo.WebAPI.Models.Search;
using SE.Neo.WebAPI.Models.Shared;

namespace SE.Neo.WebAPI.Mapping
{
    public class SearchApiProfile : Profile
    {
        public SearchApiProfile()
        {
            CreateMap<SearchEntity, SearchDocument>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => src.EntityType))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Subject))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.OriginalId ?? 0));
            CreateMap<SearchEntityCategory, BaseIdNameResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CategoryId));
            CreateMap<SearchEntityTechnology, BaseIdNameResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.TechnologyId));
            CreateMap<SearchEntitySolution, BaseIdNameResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.SolutionId));
            CreateMap<SearchEntityRegion, BaseIdNameResponse>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.RegionId));
            CreateMap<SearchEntityContentTag, BaseIdNameResponse>()
               .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.ContentTagId));
        }
    }
}

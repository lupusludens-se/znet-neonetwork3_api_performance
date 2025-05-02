using AutoMapper;
using SE.Neo.Common.Models.CMS;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Core.Enums;
using SE.Neo.WebAPI.Models.CMS;
using SE.Neo.WebAPI.Models.Country;
using SE.Neo.WebAPI.Models.Permission;
using SE.Neo.WebAPI.Models.Role;
using SE.Neo.WebAPI.Models.Shared;
using SE.Neo.WebAPI.Models.State;

namespace SE.Neo.WebAPI.Mapping
{
    public class CommonApiProfile : Profile
    {
        public CommonApiProfile()
        {
            CreateMap<RoleDTO, RoleResponse>();
            CreateMap<RoleRequest, RoleDTO>();
            CreateMap<RoleResponse, RoleDTO>();

            CreateMap<PermissionDTO, PermissionResponse>();
            CreateMap<PermissionRequest, PermissionDTO>();

            CreateMap<BaseIdNameDTO, BaseIdNameResponse>();
            CreateMap<EnumRequest<TermLengthType>, BaseIdNameDTO>();
            CreateMap<EnumRequest<PurchaseOptionType>, BaseIdNameDTO>();

            CreateMap<TaxonomyRequest, TaxonomyDTO>();

            CreateMap<ContentTagDTO, ContentTagResponse>();
            CreateMap<ContentTagResponse, ContentTagDTO>();

            CreateMap<CategoryDTO, CategoryResponse>();
            CreateMap<CategoryResponse, CategoryDTO>();
            CreateMap<CategoryRequest, CategoryDTO>();

            CreateMap<SolutionDTO, SolutionResponse>();
            CreateMap<SolutionResponse, SolutionDTO>();
            CreateMap<SolutionRequest, SolutionDTO>();

            CreateMap<TechnologyDTO, TechnologyResponse>();
            CreateMap<TechnologyResponse, TechnologyDTO>();
            CreateMap<TechnologyRequest, TechnologyDTO>();

            CreateMap<RegionDTO, RegionResponse>();
            CreateMap<RegionResponse, RegionDTO>();
            CreateMap<RegionRequest, RegionDTO>();

            CreateMap<CountryDTO, CountryResponse>();
            CreateMap<StateDTO, StateResponse>();

            CreateMap<TimeZoneDTO, TimeZoneResponse>();

            CreateMap<ResourceDTO, ResourceResponse>();
        }
    }

}
using AutoMapper;
using SE.Neo.Common.Enums;
using SE.Neo.Common.Extensions;
using SE.Neo.Common.Models.Company;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Common.Models.Tool;
using SE.Neo.WebAPI.Models.Tool;

namespace SE.Neo.WebAPI.Mapping
{
    public class ToolApiProfile : Profile
    {
        public ToolApiProfile()
        {
            CreateMap<ToolDTO, ToolResponse>();
            CreateMap<SolarQuoteRequest, SolarQuoteDTO>()
                .ForMember(dest => dest.PortfolioReview, opt => opt.MapFrom(src => false))
                .ForMember(dest => dest.CarportArea, opt => opt.Condition(src => src.CarportAvailable == true))
                .ForMember(dest => dest.CarportAreaType, opt => opt.Condition(src => src.CarportAvailable == true))
                .ForMember(dest => dest.LandArea, opt => opt.Condition(src => src.LandAvailable == true))
                .ForMember(dest => dest.LandAreaType, opt => opt.Condition(src => src.LandAvailable == true))
                .ForMember(dest => dest.RoofArea, opt => opt.Condition(src => src.RoofAvailable == true))
                .ForMember(dest => dest.RoofAreaType, opt => opt.Condition(src => src.RoofAvailable == true))
                .ForMember(dest => dest.Interests, opt => opt.MapFrom(src => src.Interests.Select(i => new BaseIdNameDTO { Id = (int)i, Name = i.GetDescription() })))
                .ForMember(dest => dest.ContractStructures, opt => opt.MapFrom(src => src.ContractStructures.Select(i => new BaseIdNameDTO { Id = (int)i, Name = i.GetDescription() })));

            CreateMap<SolarPortfolioReviewRequest, SolarQuoteDTO>()
                .ForMember(dest => dest.PortfolioReview, opt => opt.MapFrom(src => true))
                .ForMember(dest => dest.Interests, opt => opt.MapFrom(src => src.Interests.Select(i => new BaseIdNameDTO { Id = (int)i, Name = i.GetDescription() })));
            CreateMap<ToolRequest, ToolDTO>()
                .ForMember(dest => dest.Companies, opt => opt.MapFrom(src => src.CompanyIds.Select(id => new CompanyDTO { Id = id })))
                .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.RoleIds.Select(id => new RoleDTO { Id = id })))
                .ForMember(dest => dest.ToolType, opt => opt.MapFrom(src => ToolType.URL));

            CreateMap<ToolPinnedRequest, ToolPinnedDTO>();
        }
    }
}
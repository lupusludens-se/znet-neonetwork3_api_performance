using AutoMapper;
using SE.Neo.Common.Models.Tool;
using SE.Neo.Core.Entities;

namespace SE.Neo.Core.Mapping
{
    public class ToolProfile : Profile
    {
        public ToolProfile()
        {
            CreateMap<Tool, ToolDTO>()
                .ForMember(dest => dest.Roles, opts => opts.MapFrom(src => src.Roles.Select(o => o.Role)))
                .ForMember(dest => dest.Companies, opts => opts.MapFrom(src => src.Companies.Select(o => o.Company)));
            CreateMap<ToolDTO, Tool>()
                .ForMember(dest => dest.Roles, opt => opt.Ignore())
                .ForMember(dest => dest.Companies, opt => opt.Ignore());
            CreateMap<SolarQuoteDTO, SolarQuote>()
                .ForMember(dest => dest.RequestedByUser, opt => opt.Ignore())
                .ForMember(dest => dest.ContractStructures, opt => opt.Ignore())
                .ForMember(dest => dest.Interests, opt => opt.Ignore());

            CreateMap<ToolPinned, ToolPinnedDTO>();
        }
    }
}
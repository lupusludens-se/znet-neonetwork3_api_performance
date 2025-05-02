using AutoMapper;
using SE.Neo.Common.Models.Media;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Common.Models.Tool;
using SE.Neo.Core.Entities;

namespace SE.Neo.Core.Mapping
{
    public class PublicToolProfile : Profile
    {
        public PublicToolProfile()
        {
            CreateMap<Tool, ToolDTO>()
                            .ForMember(dest => dest.Pinned, opt => opt.Ignore())
                            .ForMember(dest => dest.Companies, opt => opt.Ignore()).ForMember(dest => dest.IsPinned, opt => opt.Ignore()).
                            ForMember(dest => dest.Icon, opt => opt.MapFrom(src => src.Icon)).ForMember(dest => dest.ToolUrl, opt => opt.Ignore()).
                            ForMember(dest => dest.ToolHeight, opt => opt.Ignore());
            CreateMap<Blob, BlobDTO>().ForMember(dest => dest.ContainerName, opt => opt.MapFrom(src => src.ContainerId));
            CreateMap<ToolRole, RoleDTO>();
        }
    }
}
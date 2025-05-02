using AutoMapper;
using SE.Neo.Common.Extensions;
using SE.Neo.Common.Models.Media;
using SE.Neo.Core.Enums;
using SE.Neo.WebAPI.Models.Media;

namespace SE.Neo.WebAPI.Mapping
{
    public class BlobApiProfile : Profile
    {
        public BlobApiProfile()
        {
            CreateMap<BlobDTO, BlobResponse>()
                .ForMember(dest => dest.BlobType, opt => opt.MapFrom(obj => obj.ContainerName.ToEnum<BlobType>()));
            CreateMap<BlobRequest, BlobBaseDTO>()
                .ForMember(dest => dest.ContainerName, opt => opt.MapFrom((src, dest) => dest.ContainerName = src.BlobType.ToString()))
                .ForMember(dest => dest.Name, opt => opt.MapFrom((src, dest) => dest.Name = src.BlobName ?? string.Empty));
        }
    }
}

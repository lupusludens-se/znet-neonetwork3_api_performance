using AutoMapper;
using SE.Neo.Common.Extensions;
using SE.Neo.Common.Models.Media;
using SE.Neo.Core.Entities;
using SE.Neo.Core.Enums;

namespace SE.Neo.Core.Mapping
{
    public class BlobProfile : Profile
    {
        public BlobProfile()
        {
            CreateMap<BlobBaseDTO, Blob>()
                .ForMember(dest => dest.ContainerId, opt => opt.MapFrom((src, dest) => dest.ContainerId = src.ContainerName.ToEnum<BlobType>()));
            CreateMap<Blob, BlobDTO>()
                .ForMember(dest => dest.ContainerName, opt => opt.MapFrom((src, dest) => dest.ContainerName = src.ContainerId.ToString()));
        }
    }
}

using AutoMapper;
using SE.Neo.Common.Models.Activity;
using SE.Neo.Core.Entities.TrackingActivity;

namespace SE.Neo.Core.Mapping
{
    public class PublicSiteActivityProfile : Profile
    {
        public PublicSiteActivityProfile()
        {
            CreateMap<PublicSiteActivityDTO, PublicSiteActivity>()
                .ForMember(dest => dest.TypeId, opts => opts.MapFrom(src => (Enums.ActivityType)src.TypeId))
                .ForMember(dest => dest.LocationId, opts => opts.MapFrom(src => (Enums.ActivityLocation)src.LocationId));
        }
    }
}

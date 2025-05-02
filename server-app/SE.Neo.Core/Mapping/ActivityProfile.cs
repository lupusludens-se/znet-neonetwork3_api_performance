using AutoMapper;
using SE.Neo.Common.Models.Activity;
using SE.Neo.Core.Entities.TrackingActivity;

namespace SE.Neo.Core.Mapping
{
    public class ActivityProfile : Profile
    {
        public ActivityProfile()
        {
            CreateMap<ActivityDTO, Activity>()
                .ForMember(dest => dest.TypeId, opts => opts.MapFrom(src => (Enums.ActivityType)src.TypeId))
                .ForMember(dest => dest.LocationId, opts => opts.MapFrom(src => (Enums.ActivityLocation)src.LocationId));
        }
    }
}

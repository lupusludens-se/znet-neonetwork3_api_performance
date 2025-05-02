using AutoMapper;
using SE.Neo.Common.Models.Activity;
using SE.Neo.WebAPI.Models.Activity;

namespace SE.Neo.WebAPI.Mapping
{
    public class ActivityProfile : Profile
    {
        public ActivityProfile()
        {
            CreateMap(typeof(ActivityRequest), typeof(ActivityDTO));
        }
    }
}

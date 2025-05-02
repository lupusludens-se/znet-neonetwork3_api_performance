using AutoMapper;
using SE.Neo.Common.Models.Activity;
using SE.Neo.WebAPI.Models.Activity;

namespace SE.Neo.WebAPI.Mapping
{
    public class PublicSiteActivityProfile : Profile
    {
        public PublicSiteActivityProfile()
        {
            CreateMap(typeof(ActivityRequest), typeof(PublicSiteActivityDTO));
        }
    }
}

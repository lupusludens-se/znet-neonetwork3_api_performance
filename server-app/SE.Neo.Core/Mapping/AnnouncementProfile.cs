using AutoMapper;
using SE.Neo.Common.Models.Announcement;
using SE.Neo.Core.Entities;

namespace SE.Neo.Core.Mapping
{
    public class AnnouncementProfile : Profile
    {
        public AnnouncementProfile()
        {
            CreateMap<Announcement, AnnouncementDTO>()
                .ForMember(dest => dest.Audience, opts => opts.MapFrom(src => src.Audience.Name));

            CreateMap<AnnouncementDTO, Announcement>();
        }
    }
}

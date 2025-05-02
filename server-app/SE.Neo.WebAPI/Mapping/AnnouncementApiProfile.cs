using AutoMapper;
using SE.Neo.Common.Models.Announcement;
using SE.Neo.WebAPI.Models.Announcement;

namespace SE.Neo.WebAPI.Mapping
{
    public class AnnouncementApiProfile : Profile
    {
        public AnnouncementApiProfile()
        {
            CreateMap<AnnouncementRequest, AnnouncementDTO>();

            CreateMap<AnnouncementDTO, AnnouncementResponse>();
        }
    }
}

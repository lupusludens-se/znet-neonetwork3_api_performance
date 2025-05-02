using AutoMapper;
using SE.Neo.Common.Enums;
using SE.Neo.Common.Models.Company;
using SE.Neo.Common.Models.Event;
using SE.Neo.WebAPI.Models.Event;

namespace SE.Neo.WebAPI.Mapping
{
    public class EventApiProfile : Profile
    {
        public EventApiProfile()
        {
            CreateMap<EventRequest, EventDTO>()
                .ForMember(dest => dest.Description, opt => opt.NullSubstitute(string.Empty))
                .ForPath(dest => dest.Links, opt => opt.Ignore());
            CreateMap<EventLinkRequest, EventLinkDTO>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => EventLinkType.Link));
            CreateMap<EventRecordingRequest, EventLinkDTO>()
                .ForMember(dest => dest.Type, opt => opt.MapFrom(src => EventLinkType.Recording));
            CreateMap<EventModeratorRequest, EventModeratorDTO>();
            CreateMap<EventOccurrenceRequest, EventOccurrenceDTO>();
            CreateMap<EventCompanyRequest, CompanyDTO>();

            CreateMap<EventDTO, EventResponse>()
                .ForMember(dest => dest.Links, opt => opt.MapFrom(src => src.Links.Where(x => x.Type == EventLinkType.Link)))
                .ForMember(dest => dest.Recordings, opt => opt.MapFrom(src => src.Links.Where(x => x.Type == EventLinkType.Recording)));
            CreateMap<EventLinkDTO, EventRecordingResponse>();
            CreateMap<EventLinkDTO, EventLinkResponse>();
            CreateMap<EventModeratorDTO, EventModeratorResponse>();
            CreateMap<EventUserDTO, EventUserResponse>();
            CreateMap<EventOccurrenceDTO, EventOccurrenceResponse>();
            CreateMap<EventUserRequest, EventUserDTO>();
            CreateMap<EventMatchingUserDTO, EventMatchingUserResponse>();
        }
    }
}

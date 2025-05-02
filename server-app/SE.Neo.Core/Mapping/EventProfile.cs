using AutoMapper;
using SE.Neo.Common.Models.CMS;
using SE.Neo.Common.Models.Event;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Core.Entities;
using SE.Neo.Core.Models.Event;

namespace SE.Neo.Core.Mapping
{
    public class EventProfile : Profile
    {
        public EventProfile()
        {
            CreateMap<EventDTO, Event>()
                .ForMember(dest => dest.EventCategories, opt => opt.Ignore())
                .ForMember(dest => dest.EventLinks, opt => opt.Ignore())
                .ForMember(dest => dest.EventModerators, opt => opt.Ignore())
                .ForMember(dest => dest.EventOccurrences, opt => opt.Ignore())
                .ForMember(dest => dest.EventInvitedRoles, opt => opt.Ignore())
                .ForMember(dest => dest.EventInvitedRegions, opt => opt.Ignore())
                .ForMember(dest => dest.EventInvitedCategories, opt => opt.Ignore())
                .ForMember(dest => dest.EventInvitedUsers, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(dest => dest.Categories, opt => opt.MapFrom(src => src.EventCategories.Where(x => x.Category != null).Select(x => x.Category)))
                .ForMember(dest => dest.Attendees, opt => opt.MapFrom(src => src.EventAttendees.Where(x => x.User != null && x.IsAttending).Select(x => x.User)))
                .ForMember(dest => dest.Links, opt => opt.MapFrom(src => src.EventLinks))
                .ForMember(dest => dest.Moderators, opt => opt.MapFrom(src => src.EventModerators))
                .ForMember(dest => dest.Occurrences, opt => opt.MapFrom(src => src.EventOccurrences))
                .ForMember(dest => dest.InvitedUsers, opt => opt.MapFrom(src => src.EventInvitedUsers.Where(x => x.User != null).Select(x => x.User)))
                .ForMember(dest => dest.InvitedCategories, opt => opt.MapFrom(src => src.EventInvitedCategories.Where(x => x.Category != null).Select(x => x.Category)))
                .ForMember(dest => dest.InvitedRegions, opt => opt.MapFrom(src => src.EventInvitedRegions.Where(x => x.Region != null).Select(x => x.Region)))
                .ForMember(dest => dest.InvitedRoles, opt => opt.MapFrom(src => src.EventInvitedRoles.Where(x => x.Role != null).Select(x => x.Role)));

            CreateMap<CategoryDTO, EventCategory>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.Id))
                .ReverseMap()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CategoryId));

            CreateMap<EventLinkDTO, EventLink>()
                .ReverseMap();

            CreateMap<EventModeratorDTO, EventModerator>()
                .ReverseMap()
                .ForMember(dest => dest.User, opt => opt.MapFrom(src => src.User));

            CreateMap<EventOccurrenceDTO, EventOccurrence>()
                .ReverseMap();

            CreateMap<User, EventUserDTO>()
                .ForMember(dest => dest.Name, opts => opts.MapFrom(src => $"{src.FirstName} {src.LastName}"))
                .ForMember(dest => dest.Company, opts => opts.MapFrom(src => src.Company.Name));

            CreateMap<CategoryDTO, EventInvitedCategory>()
                .ForMember(dest => dest.Id, opts => opts.Ignore())
                .ForMember(dest => dest.CategoryId, opts => opts.MapFrom(src => src.Id))
                .ReverseMap()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.CategoryId));

            CreateMap<RegionDTO, EventInvitedRegion>()
                .ForMember(dest => dest.Id, opts => opts.Ignore())
                .ForMember(dest => dest.RegionId, opts => opts.MapFrom(src => src.Id))
                .ReverseMap()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.RegionId));

            CreateMap<RoleDTO, EventInvitedRole>()
                .ForMember(dest => dest.Id, opts => opts.Ignore())
                .ForMember(dest => dest.RoleId, opts => opts.MapFrom(src => src.Id))
                .ReverseMap()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.RoleId));


            CreateMap<EventUserDTO, EventInvitedUser>()
                .ForMember(dest => dest.Id, opts => opts.Ignore())
                .ForMember(dest => dest.UserId, opts => opts.MapFrom(src => src.Id))
                .ReverseMap()
                .ForMember(dest => dest.Id, opts => opts.MapFrom(src => src.UserId));


            CreateMap<EventUser, EventMatchingUserDTO>();
        }
    }
}
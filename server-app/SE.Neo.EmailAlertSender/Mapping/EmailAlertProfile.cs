using AutoMapper;
using SE.Neo.Common.Enums;
using SE.Neo.Common.Models.Media;
using SE.Neo.Core.Entities;
using SE.Neo.EmailAlertSender.Models;
using SE.Neo.EmailTemplates.Models;

namespace SE.Neo.EmailAlertSender.Mapping
{
    public class EmailAlertProfile : Profile
    {
        public EmailAlertProfile()
        {
            CreateMap<EventInvitationItem, EventInvitationEmailTemplatedModel>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.UserFirstName))
                .ForMember(dest => dest.EventHighlights, opt => { opt.PreCondition(src => src.EventHighlights != null); opt.MapFrom(src => src.EventHighlights.Split('\n', StringSplitOptions.None)); })
                .ForMember(dest => dest.EventType, opt => opt.MapFrom(src => src.EventType == EventLocationType.InPerson ? "In Person Event" : "Virtual Event"));
            CreateMap<EventReminderItem, EventInvitationEmailTemplatedModel>()
                .ForMember(dest => dest.EventHighlights, opt => opt.MapFrom(src => src.EventHighlights.Split('\n', StringSplitOptions.None)));
            CreateMap<ForumResponseItem, ForumResponseEmailTemplatedModel>()
                .ForMember(dest => dest.ResponseText, opt => { opt.PreCondition(src => src.Messages.Count() == 1); opt.MapFrom(src => src.Messages.Single().MessageText); })
                .ForMember(dest => dest.ResponseAuthor, opt => { opt.PreCondition(src => src.Messages.Count() == 1); opt.MapFrom(src => src.Messages.Single().MessageAuthor); })
                .ForMember(dest => dest.ResponseCount, opt => opt.MapFrom(src => src.Messages.Count()));
            CreateMap<SummaryAlertItem, SummaryEmailItem>()
                .ForMember(dest => dest.MainText, opt => opt.MapFrom(src => src.EventLocationType != null ? src.EventLocationType + "\n" + src.MainText : src.MainText));
            CreateMap<EventAlertDateInfo, EventDateInfo>();
            CreateMap<UnreadMessageNotificationItem, NewMessageEmailTemplatedModel>();
            CreateMap<User, CompleteProfileEmailTemplatedModel>();
            CreateMap<Blob, BlobBaseDTO>()
               .ForMember(dest => dest.ContainerName, opt => opt.MapFrom((src, dest) => dest.ContainerName = src.ContainerId.ToString()));
            CreateMap<BlobBaseDTO, BlobDTO>()
               .ForMember(dest => dest.ContainerName, opt => opt.MapFrom((src, dest) => dest.ContainerName = src.ContainerName.ToString()));
        }
    }
}
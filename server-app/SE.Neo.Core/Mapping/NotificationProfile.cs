using AutoMapper;
using SE.Neo.Common.Models.Notifications;
using SE.Neo.Core.Entities;

namespace SE.Neo.Core.Mapping
{
    public class NotificationProfile : Profile
    {
        public NotificationProfile()
        {
            CreateMap<UserNotification, NotificationDTO>();

            CreateMap<NotificationDTO, UserNotification>()
                .ForMember(dest => dest.Details, opts => opts.MapFrom(src => src.DetailsAsString));

        }
    }
}

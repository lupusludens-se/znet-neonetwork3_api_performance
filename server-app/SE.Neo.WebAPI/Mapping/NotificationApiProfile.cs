using AutoMapper;
using SE.Neo.Common.Models.Notifications;
using SE.Neo.WebAPI.Models.Notification;

namespace SE.Neo.WebAPI.Mapping
{
    public class NotificationApiProfile : Profile
    {
        public NotificationApiProfile()
        {
            CreateMap<NotificationDTO, NotificationResponse>();
        }
    }
}

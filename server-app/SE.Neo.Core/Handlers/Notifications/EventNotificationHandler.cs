using AutoMapper;
using SE.Neo.Common.Enums;
using SE.Neo.Common.Models.Notifications;
using SE.Neo.Common.Models.Notifications.Details.Base;
using SE.Neo.Common.Models.Notifications.Details.Single;
using SE.Neo.Core.Data;
using SE.Neo.Core.Entities;
using System.Text.Json;

namespace SE.Neo.Core.Handlers.Notifications
{
    internal class EventNotificationHandler : BaseNotificationHandler
    {
        public EventNotificationHandler(ApplicationContext context, IMapper mapper, NotificationType type)
            : base(context, mapper, type)
        {
        }

        public override async Task<int> AddAsync(int userId, ISingleNotificationDetails details)
        {
            if (details is EventNotificationDetails)
            {
                IEnumerable<UserNotification>? unreadUserNotifications = null;
                if (_type == NotificationType.ChangesToEventIInvited)
                {
                    unreadUserNotifications = GetUnreadUserNotificationsByType(userId, _type)
                        .Where(n => JsonSerializer.Deserialize<EventNotificationDetails>(n.Details)!.EventId ==
                                    ((EventNotificationDetails)details).EventId);
                }

                return await InsertOrUpdateNotification(new NotificationDTO(userId, _type, details),
                    (currentDetails, type) =>
                    {
                        if (type == NotificationType.ChangesToEventIInvited)
                        {
                            return new EventNotificationDetails
                            {
                                EventId = (currentDetails as EventNotificationDetails)!.EventId,
                                EventTitle = (currentDetails as EventNotificationDetails)!.EventTitle
                            };
                        }
                        return null;
                    },
                    unreadUserNotifications);
            }

            ThrowNotificationDetailsAreNotValid(details);
            return 0;
        }
    }
}

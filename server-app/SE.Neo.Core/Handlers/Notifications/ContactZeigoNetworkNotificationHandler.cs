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
    internal class ContactZeigoNetworkNotificationHandler : BaseNotificationHandler
    {
        public ContactZeigoNetworkNotificationHandler(ApplicationContext context, IMapper mapper, NotificationType type)
            : base(context, mapper, type)
        {
        }
        public override async Task<int> AddAsync(int userId, ISingleNotificationDetails details)
        {
            if (details is ContactZeigoNetworkMessageNotificationDetails)
            {
                IEnumerable<UserNotification> unreadUserNotifications = GetUnreadUserNotificationsByType(userId, _type)
                    .Where(n => JsonSerializer.Deserialize<ContactZeigoNetworkMessageNotificationDetails>(n.Details)!.ConversationId ==
                                ((ContactZeigoNetworkMessageNotificationDetails)details).ConversationId);

                return await InsertOrUpdateNotification(new NotificationDTO(userId, _type, details),
                    (currentDetails, _) =>
                    {
                        return new ContactZeigoNetworkMessageNotificationDetails
                        {
                            ConversationId = (currentDetails as ContactZeigoNetworkMessageNotificationDetails)!.ConversationId
                        };
                    }, unreadUserNotifications);
            }

            ThrowNotificationDetailsAreNotValid(details);
            return 0;
        }
    }
}

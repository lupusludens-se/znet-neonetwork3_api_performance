using AutoMapper;
using SE.Neo.Common.Enums;
using SE.Neo.Common.Models.Notifications;
using SE.Neo.Common.Models.Notifications.Details.Base;
using SE.Neo.Common.Models.Notifications.Details.Multiple;
using SE.Neo.Common.Models.Notifications.Details.Single;
using SE.Neo.Core.Data;
using SE.Neo.Core.Entities;
using System.Text.Json;

namespace SE.Neo.Core.Handlers.Notifications
{
    internal class MessageNotificationHandler : BaseNotificationHandler
    {
        public MessageNotificationHandler(ApplicationContext context, IMapper mapper, NotificationType type)
            : base(context, mapper, type)
        {
        }

        public override async Task<int> AddAsync(int userId, ISingleNotificationDetails details)
        {
            if (details is MessageNotificationDetails)
            {
                IEnumerable<UserNotification> unreadUserNotifications = GetUnreadUserNotificationsByType(userId, _type)
                    .Where(n => JsonSerializer.Deserialize<MessageNotificationDetails>(n.Details)!.ConversationId ==
                                ((MessageNotificationDetails)details).ConversationId);

                return await InsertOrUpdateNotification(new NotificationDTO(userId, _type, details),
                    (currentDetails, _) =>
                    {
                        return new GroupMessagesNotificationDetails
                        {
                            ConversationId = (currentDetails as MessageNotificationDetails)!.ConversationId,
                            Count = IncrementCount(currentDetails),
                        };
                    }, unreadUserNotifications);
            }

            ThrowNotificationDetailsAreNotValid(details);
            return 0;
        }
    }
}

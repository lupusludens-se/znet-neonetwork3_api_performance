using AutoMapper;
using SE.Neo.Common.Enums;
using SE.Neo.Common.Models.Notifications;
using SE.Neo.Common.Models.Notifications.Details.Base;
using SE.Neo.Common.Models.Notifications.Details.Multiple;
using SE.Neo.Core.Data;
using SE.Neo.Core.Entities;
using System.Text.Json;

namespace SE.Neo.Core.Handlers.Notifications
{
    internal class ForumNotificationHandler : BaseNotificationHandler
    {
        public ForumNotificationHandler(ApplicationContext context, IMapper mapper, NotificationType type)
            : base(context, mapper, type)
        {
        }

        public override async Task<int> AddAsync(int userId, ISingleNotificationDetails details)
        {
            if (details is TopicNotificationDetails)
            {
                IEnumerable<UserNotification> unreadUserNotifications = GetUnreadUserNotificationsByType(userId, _type)
                    .Where(n => JsonSerializer.Deserialize<TopicNotificationDetails>(n.Details)!.TopicId ==
                                ((TopicNotificationDetails)details).TopicId);

                return await InsertOrUpdateNotification(new NotificationDTO(userId, _type, details),
                    (currentDetails, _) =>
                    {
                        return new GroupUsersTopicNotificationDetails
                        {
                            TopicId = (currentDetails as TopicNotificationDetails)!.TopicId,
                            TopicTitle = (currentDetails as TopicNotificationDetails)!.TopicTitle,
                            Count = IncrementCount(currentDetails),
                        };
                    }, unreadUserNotifications);
            }

            ThrowNotificationDetailsAreNotValid(details);
            return 0;
        }
    }
}

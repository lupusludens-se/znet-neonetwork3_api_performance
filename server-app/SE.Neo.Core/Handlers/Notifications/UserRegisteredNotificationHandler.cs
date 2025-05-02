using AutoMapper;
using SE.Neo.Common.Enums;
using SE.Neo.Common.Models.Notifications;
using SE.Neo.Common.Models.Notifications.Details.Base;
using SE.Neo.Common.Models.Notifications.Details.Multiple;
using SE.Neo.Common.Models.Notifications.Details.Single;
using SE.Neo.Core.Data;
using SE.Neo.Core.Entities;

namespace SE.Neo.Core.Handlers.Notifications
{
    internal class UserRegisteredNotificationHandler : BaseNotificationHandler
    {
        public UserRegisteredNotificationHandler(ApplicationContext context, IMapper mapper, NotificationType type)
            : base(context, mapper, type)
        {
        }

        public override async Task<int> AddAsync(int userId, ISingleNotificationDetails details)
        {
            if (details is UserRegisteredNotificationDetails)
            {
                IEnumerable<UserNotification> unreadUserNotifications = GetUnreadUserNotificationsByType(userId, _type);

                return await InsertOrUpdateNotification(new NotificationDTO(userId, _type, details),
                    (currentDetails, _) =>
                    {
                        return new GroupUserRegisteredNotificationDetails
                        {
                            Count = IncrementCount(currentDetails)
                        };
                    }, unreadUserNotifications);
            }

            ThrowNotificationDetailsAreNotValid(details);
            return 0;
        }
    }
}

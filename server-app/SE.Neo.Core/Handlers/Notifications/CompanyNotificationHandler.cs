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
    internal class CompanyNotificationHandler : BaseNotificationHandler
    {
        public CompanyNotificationHandler(ApplicationContext context, IMapper mapper, NotificationType type)
            : base(context, mapper, type)
        {
        }

        public override async Task<int> AddAsync(int userId, ISingleNotificationDetails details)
        {
            if (details is CompanyNotificationDetails)
            {
                IEnumerable<UserNotification> unreadUserNotifications = GetUnreadUserNotificationsByType(userId, _type)
                    .Where(n => JsonSerializer.Deserialize<CompanyNotificationDetails>(n.Details)!.CompanyId ==
                                ((CompanyNotificationDetails)details).CompanyId);

                return await InsertOrUpdateNotification(new NotificationDTO(userId, _type, details),
                    (currentDetails, _) =>
                    {
                        return new CompanyNotificationDetails
                        {
                            CompanyId = (currentDetails as CompanyNotificationDetails)!.CompanyId,
                            CompanyName = (currentDetails as CompanyNotificationDetails)!.CompanyName
                        };
                    }, unreadUserNotifications);
            }

            ThrowNotificationDetailsAreNotValid(details);
            return 0;
        }
    }
}

using AutoMapper;
using SE.Neo.Common.Enums;
using SE.Neo.Common.Models.Notifications;
using SE.Neo.Common.Models.Notifications.Details.Base;
using SE.Neo.Common.Models.Notifications.Details.Single;
using SE.Neo.Core.Data;

namespace SE.Neo.Core.Handlers.Notifications
{
    internal class AlertNotificationHandler : BaseNotificationHandler
    {
        public AlertNotificationHandler(ApplicationContext context, IMapper mapper, NotificationType type)
            : base(context, mapper, type)
        {
        }

        public override async Task<int> AddAsync(int userId, ISingleNotificationDetails details)
        {
            if (details is AdminAlertNotificationDetails)
            {
                return await InsertOrUpdateNotification(new NotificationDTO(userId, _type, details));
            }

            ThrowNotificationDetailsAreNotValid(details);
            return 0;
        }
    }
}

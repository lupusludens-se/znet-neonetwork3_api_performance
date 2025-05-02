using AutoMapper;
using SE.Neo.Common.Enums;
using SE.Neo.Common.Models.Notifications;
using SE.Neo.Common.Models.Notifications.Details.Base;
using SE.Neo.Core.Data;

namespace SE.Neo.Core.Handlers.Notifications
{
    public class NewForumNotificationHandler : BaseNotificationHandler
    {
        public NewForumNotificationHandler(ApplicationContext context, IMapper mapper, NotificationType type)
            : base(context, mapper, type)
        {
        }

        public override async Task<int> AddAsync(int userId, ISingleNotificationDetails details)
        {
            if (details is TopicNotificationDetails)
            {
                return await InsertOrUpdateNotification(new NotificationDTO(userId, _type, details));
            }

            ThrowNotificationDetailsAreNotValid(details);
            return 0;
        }

        public async Task<int> AddRangeAsync(List<int> userId, ISingleNotificationDetails details)
        {
            IEnumerable<NotificationDTO> notificationDTOs = userId.Select(id => new NotificationDTO(id, _type, details));
            if (details is TopicNotificationDetails)
            {
                return await InsertNotificationRange(notificationDTOs, _type);
            }

            ThrowNotificationDetailsAreNotValid(details);
            return 0;
        }
    }
}

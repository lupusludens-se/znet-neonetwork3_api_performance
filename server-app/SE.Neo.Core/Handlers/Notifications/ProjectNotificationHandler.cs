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
    internal class ProjectNotificationHandler : BaseNotificationHandler
    {
        public ProjectNotificationHandler(ApplicationContext context, IMapper mapper, NotificationType type)
            : base(context, mapper, type)
        {
        }

        public override async Task<int> AddAsync(int userId, ISingleNotificationDetails details)
        {
            if (details is ProjectNotificationDetails)
            {
                IEnumerable<UserNotification> unreadUserNotifications = GetUnreadUserNotificationsByType(userId, _type)
                    .Where(n => JsonSerializer.Deserialize<ProjectNotificationDetails>(n.Details)!.ProjectId ==
                                ((ProjectNotificationDetails)details).ProjectId);

                return await InsertOrUpdateNotification(new NotificationDTO(userId, _type, details),
                    (currentDetails, _) =>
                    {
                        return new ProjectNotificationDetails
                        {
                            ProjectId = (currentDetails as ProjectNotificationDetails)!.ProjectId,
                            ProjectTitle = (currentDetails as ProjectNotificationDetails)!.ProjectTitle
                        };
                    }, unreadUserNotifications);
            }

            ThrowNotificationDetailsAreNotValid(details);
            return 0;
        }
    }
}

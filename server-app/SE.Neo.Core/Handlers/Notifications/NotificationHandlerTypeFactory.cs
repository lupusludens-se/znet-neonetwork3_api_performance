using SE.Neo.Common.Attributes;
using SE.Neo.Common.Enums;
namespace SE.Neo.Core.Handlers.Notifications
{
    public static class NotificationHandlerTypeFactory
    {
        public static Type GetHandlerType(NotificationType notificationType)
        {
            switch (notificationType)
            {
                case NotificationType.CommentsMyTopic:
                case NotificationType.LikesMyTopic:
                case NotificationType.RepliesToMyComment:
                case NotificationType.RepliesToTopicIFollow:
                case NotificationType.MentionsMeInComment:
                    return typeof(ForumNotificationHandler);
                case NotificationType.InvitesMeToEvent:
                case NotificationType.ChangesToEventIInvited:
                    return typeof(EventNotificationHandler);
                case NotificationType.FollowsMe:
                    return typeof(FollowerNotificationHandler);
                case NotificationType.MessagesMe:
                    return typeof(MessageNotificationHandler);
                case NotificationType.AdminAlert:
                    return typeof(AlertNotificationHandler);
                case NotificationType.ChangesToProjectISaved:
                    return typeof(ProjectNotificationHandler);
                case NotificationType.CompanyIFollowPostProject:
                    return typeof(CompanyProjectNotificationHandler);
                case NotificationType.UserRegistered:
                    return typeof(UserRegisteredNotificationHandler);
                case NotificationType.CompanyIFollowAddEmployee:
                    return typeof(CompanyNotificationHandler);
                case NotificationType.UserDeleted:
                    return typeof(UserDeletedNotificationHandler);
                case NotificationType.NewForumCreated:
                    return typeof(NewForumNotificationHandler);
                case NotificationType.NewPrivateForumCreated:
                    return typeof(NewForumNotificationHandler);
                case NotificationType.UserAutoApproved:
                    return typeof(UserAutoApprovedNotificationHandler);
                case NotificationType.ContactZeigoNetwork:
                    return typeof(ContactZeigoNetworkNotificationHandler);
                case NotificationType.NewFeedback:
                    return typeof(FeedbackNotificationHandler);
                case NotificationType.NewInitiativeCreated:
                    return typeof(InitiativeNotificationHandler);
                default:
                    throw new CustomException($"Can't handle the notification. Notification type {notificationType} is not supported.");
            }
        }
    }
}

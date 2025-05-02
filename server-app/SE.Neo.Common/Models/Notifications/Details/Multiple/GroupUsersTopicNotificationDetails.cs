using SE.Neo.Common.Models.Notifications.Details.Base;

namespace SE.Neo.Common.Models.Notifications.Details.Multiple
{
    // N2, N4, N6, N8, N10
    public class GroupUsersTopicNotificationDetails : TopicNotificationDetails, INotificationDetails, IGrouppedNotificationDetails
    {
        public int Count { get; set; }
    }
}

using SE.Neo.Common.Models.Notifications.Details.Base;

namespace SE.Neo.Common.Models.Notifications.Details.Single
{
    // N1, N3, N5, N7, N9
    public class UserTopicNotificationDetails : TopicNotificationDetails, ISingleNotificationDetails, INotificationDetails
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
    }
}

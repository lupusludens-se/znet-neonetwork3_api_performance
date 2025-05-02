namespace SE.Neo.Common.Models.Notifications.Details.Base
{
    public class TopicNotificationDetails : BaseNotificationDetails, ISingleNotificationDetails, INotificationDetails
    {
        public int TopicId { get; set; }
        public string TopicTitle { get; set; }
    }
}

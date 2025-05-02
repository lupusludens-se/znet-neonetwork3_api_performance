using SE.Neo.Common.Models.Notifications.Details.Base;

namespace SE.Neo.Common.Models.Notifications.Details.Single
{
    // N11, N13
    public class EventNotificationDetails : BaseNotificationDetails, ISingleNotificationDetails, INotificationDetails
    {
        public int EventId { get; set; }
        public string EventTitle { get; set; }
    }
}

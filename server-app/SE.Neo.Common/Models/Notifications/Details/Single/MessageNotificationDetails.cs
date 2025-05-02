using SE.Neo.Common.Models.Notifications.Details.Base;

namespace SE.Neo.Common.Models.Notifications.Details.Single
{
    // N16
    public class MessageNotificationDetails : BaseNotificationDetails, ISingleNotificationDetails, INotificationDetails
    {
        public int ConversationId { get; set; }
        public int MessageId { get; set; }
    }
}

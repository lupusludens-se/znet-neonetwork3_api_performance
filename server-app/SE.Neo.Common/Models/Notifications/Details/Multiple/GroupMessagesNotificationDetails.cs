using SE.Neo.Common.Models.Notifications.Details.Base;
using SE.Neo.Common.Models.Notifications.Details.Single;

namespace SE.Neo.Common.Models.Notifications.Details.Multiple
{
    // N17
    public class GroupMessagesNotificationDetails : MessageNotificationDetails, INotificationDetails, IGrouppedNotificationDetails
    {
        public int Count { get; set; }
    }
}

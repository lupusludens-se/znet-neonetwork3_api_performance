using SE.Neo.Common.Models.Notifications.Details.Base;
using SE.Neo.Common.Models.Notifications.Details.Single;

namespace SE.Neo.Common.Models.Notifications.Details.Multiple
{
    // N24
    public class GroupUserRegisteredNotificationDetails : UserRegisteredNotificationDetails, IGrouppedNotificationDetails, INotificationDetails
    {
        public int Count { get; set; }
    }
}

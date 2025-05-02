using SE.Neo.Common.Models.Notifications.Details.Base;

namespace SE.Neo.Common.Models.Notifications.Details.Multiple
{
    // N15
    public class GroupFollowersNotificationDetails : BaseFollowerNotificationDetails, INotificationDetails, IGrouppedNotificationDetails
    {
        public int Count { get; set; }
    }
}

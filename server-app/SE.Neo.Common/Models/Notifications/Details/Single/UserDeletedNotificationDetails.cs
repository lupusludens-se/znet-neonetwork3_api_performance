using SE.Neo.Common.Models.Notifications.Details.Base;

namespace SE.Neo.Common.Models.Notifications.Details.Single
{
    // N27
    public class UserDeletedNotificationDetails : BaseNotificationDetails, ISingleNotificationDetails, INotificationDetails
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
    }
}

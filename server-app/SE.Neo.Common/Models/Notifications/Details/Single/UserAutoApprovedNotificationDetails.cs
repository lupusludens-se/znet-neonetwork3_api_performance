using SE.Neo.Common.Models.Notifications.Details.Base;

namespace SE.Neo.Common.Models.Notifications.Details.Single
{
    public class UserAutoApprovedNotificationDetails : BaseNotificationDetails, ISingleNotificationDetails, INotificationDetails
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string CompanyName { get; set; }
    }
}

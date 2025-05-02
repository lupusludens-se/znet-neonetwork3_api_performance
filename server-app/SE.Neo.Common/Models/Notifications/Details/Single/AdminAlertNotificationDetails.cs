using SE.Neo.Common.Models.Notifications.Details.Base;

namespace SE.Neo.Common.Models.Notifications.Details.Single
{
    // N18
    public class AdminAlertNotificationDetails : BaseNotificationDetails, ISingleNotificationDetails, INotificationDetails
    {
        public string AlertMessage { get; set; }
    }
}

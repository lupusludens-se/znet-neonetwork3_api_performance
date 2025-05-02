using SE.Neo.Common.Models.Notifications.Details.Base;

namespace SE.Neo.Common.Models.Notifications.Details.Multiple
{
    // N26
    public class CompanyNotificationDetails : BaseNotificationDetails, INotificationDetails
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
    }
}

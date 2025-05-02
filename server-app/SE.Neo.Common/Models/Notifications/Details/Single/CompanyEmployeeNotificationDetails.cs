using SE.Neo.Common.Models.Notifications.Details.Base;
using SE.Neo.Common.Models.Notifications.Details.Multiple;

namespace SE.Neo.Common.Models.Notifications.Details.Single
{
    // N25
    public class CompanyEmployeeNotificationDetails : CompanyNotificationDetails, ISingleNotificationDetails, INotificationDetails
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
    }
}

using SE.Neo.Common.Models.Notifications.Details.Base;
using SE.Neo.Common.Models.Notifications.Details.Multiple;

namespace SE.Neo.Common.Models.Notifications.Details.Single
{
    // N21
    public class CompanyProjectNotificationDetails : CompanyNotificationDetails, ISingleNotificationDetails, INotificationDetails
    {
        public int ProjectId { get; set; }
        public string ProjectTitle { get; set; }
    }
}
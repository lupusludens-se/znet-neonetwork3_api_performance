using SE.Neo.Common.Models.Notifications.Details.Base;

namespace SE.Neo.Common.Models.Notifications.Details.Multiple
{
    // N22
    public class GroupCompanyProjectNotificationDetails : CompanyNotificationDetails, IGrouppedNotificationDetails, INotificationDetails
    {
        public int Count { get; set; }
    }
}

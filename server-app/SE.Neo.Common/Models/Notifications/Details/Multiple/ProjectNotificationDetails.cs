using SE.Neo.Common.Models.Notifications.Details.Base;

namespace SE.Neo.Common.Models.Notifications.Details.Multiple
{
    // N20
    public class ProjectNotificationDetails : BaseNotificationDetails, ISingleNotificationDetails, INotificationDetails
    {
        public int ProjectId { get; set; }
        public string ProjectTitle { get; set; }
    }
}

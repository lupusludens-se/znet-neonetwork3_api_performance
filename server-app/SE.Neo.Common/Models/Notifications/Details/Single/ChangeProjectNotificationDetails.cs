using SE.Neo.Common.Models.Notifications.Details.Base;
using SE.Neo.Common.Models.Notifications.Details.Multiple;

namespace SE.Neo.Common.Models.Notifications.Details.Single
{
    // N19
    public class ChangeProjectNotificationDetails : ProjectNotificationDetails, ISingleNotificationDetails, INotificationDetails
    {
        public string FieldName { get; set; }
    }
}

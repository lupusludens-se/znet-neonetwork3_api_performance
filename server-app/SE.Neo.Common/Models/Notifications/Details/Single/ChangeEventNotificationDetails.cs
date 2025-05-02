using SE.Neo.Common.Models.Notifications.Details.Base;

namespace SE.Neo.Common.Models.Notifications.Details.Single
{
    // N12
    public class ChangeEventNotificationDetails : EventNotificationDetails, ISingleNotificationDetails, INotificationDetails
    {
        public string FieldName { get; set; }
    }
}

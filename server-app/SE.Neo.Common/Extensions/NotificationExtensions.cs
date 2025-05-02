using SE.Neo.Common.Models.Notifications.Details.Base;
using System.Text.Json;

namespace SE.Neo.Common.Extensions
{
    public static class NotificationExtensions
    {
        public static INotificationDetails? ToNotificationDetailsObject(this string details)
        {
            BaseNotificationDetails? baseDetails = JsonSerializer.Deserialize<BaseNotificationDetails>(details);
            if (baseDetails != null)
            {
                Type? currentDetailsType = typeof(INotificationDetails).Assembly.GetType(baseDetails.Type);
                if (currentDetailsType != null)
                {
                    return (INotificationDetails)JsonSerializer.Deserialize(details, currentDetailsType)!;
                }
            }
            return null;
        }
    }
}

using SE.Neo.Common.Enums;

namespace SE.Neo.WebAPI.Models.Notification
{
    public class NotificationResponse
    {
        public int Id { get; set; }

        public NotificationType Type { get; set; }

        public bool IsRead { get; set; }

        public bool IsSeen { get; set; }

        public object? Details { get; set; }

        public DateTime DetailsModifiedTime { get; set; }
        public DateTime ModifiedOn { get; set; }
    }
}

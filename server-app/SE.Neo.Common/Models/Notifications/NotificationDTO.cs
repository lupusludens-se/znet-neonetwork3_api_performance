using SE.Neo.Common.Enums;
using SE.Neo.Common.Extensions;
using SE.Neo.Common.Models.Notifications.Details.Base;
using SE.Neo.Common.Models.User;
using System.Text.Json;

namespace SE.Neo.Common.Models.Notifications
{
    public class NotificationDTO
    {
        public NotificationDTO() { }
        public NotificationDTO(int userId, NotificationType type, INotificationDetails details)
        {
            this.UserId = userId;
            this.Type = type;
            this.Details = details;
            this.DetailsAsString = JsonSerializer.Serialize((object)details);
        }

        public NotificationDTO(int id, int userId, UserDTO user, NotificationType type, bool isRead, bool isSeen, string details, DateTime detailsModifiedTime, DateTime modifiedOn)
        {
            Id = id;
            UserId = userId;
            User = user;
            Type = type;
            IsRead = isRead;
            IsSeen = isSeen;
            Details = details.ToNotificationDetailsObject();
            DetailsAsString = details;
            DetailsModifiedTime = detailsModifiedTime;
            ModifiedOn = modifiedOn;
        }

        public int Id { get; set; }

        public int UserId { get; set; }

        public UserDTO User { get; set; }

        public NotificationType Type { get; set; }

        public bool IsRead { get; set; }
        public bool IsSeen { get; set; }

        public INotificationDetails? Details { get; set; }
        public string DetailsAsString { get; set; }

        public DateTime DetailsModifiedTime { get; set; } = DateTime.UtcNow;

        public DateTime ModifiedOn { get; set; }
    }
}

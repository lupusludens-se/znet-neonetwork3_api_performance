using Microsoft.EntityFrameworkCore;
using SE.Neo.Common.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("User_Notification")]
    [Index(nameof(UserId), nameof(IsRead))]
    [Index(nameof(UserId), nameof(IsSeen))]
    public class UserNotification : BaseIdEntity
    {
        [Column("User_Notification_Id")]
        public override int Id { get; set; }

        [Column("User_Id")]
        [ForeignKey("User")]
        public int UserId { get; set; }

        public User User { get; set; }

        [Column("Notification_Type")]
        public NotificationType Type { get; set; }

        [Column("Is_Read")]
        public bool IsRead { get; set; }

        [Column("Is_Seen")]
        public bool IsSeen { get; set; }

        public string Details { get; set; }

        [Column("Details_Modified_Time")]
        public DateTime DetailsModifiedTime { get; set; } = DateTime.UtcNow;

        [NotMapped]
        public bool SupportGroupping
        {
            get
            {
                return Type != NotificationType.InvitesMeToEvent && Type != NotificationType.AdminAlert
                    && Type != NotificationType.UserDeleted;
            }
        }
    }
}

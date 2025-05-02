using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("Event_Invited_User")]
    [Index(nameof(EventId), nameof(UserId), IsUnique = true)]
    public class EventInvitedUser : BaseIdEntity
    {
        [Column("Event_Invited_User_Id")]
        public override int Id { get; set; }

        [Column("User_Id")]
        [ForeignKey("User")]
        public int UserId { get; set; }

        public User User { get; set; }

        [Column("Event_Id")]
        [ForeignKey("Event")]
        public int EventId { get; set; }

        public Event Event { get; set; }

        public bool? IsFirstTimeEmail { get; set; } = null;
    }
}

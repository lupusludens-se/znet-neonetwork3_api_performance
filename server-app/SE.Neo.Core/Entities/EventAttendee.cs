using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("Event_Attendee")]
    [Index(nameof(UserId), nameof(EventId), IsUnique = true)]
    public class EventAttendee : BaseIdEntity
    {
        [Column("Event_Attendee_Id")]
        public override int Id { get; set; }

        [Column("Is_Attending")]
        public bool IsAttending { get; set; }

        [Column("User_Id")]
        [ForeignKey("User")]
        public int UserId { get; set; }

        public User User { get; set; }

        [Column("Event_Id")]
        [ForeignKey("Event")]
        public int EventId { get; set; }

        public Event Event { get; set; }
    }
}

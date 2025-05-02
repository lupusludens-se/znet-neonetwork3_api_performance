using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("Event_Moderator")]
    public class EventModerator : BaseIdEntity
    {
        [Column("Event_Moderator_Id")]
        public override int Id { get; set; }

        [Column("Name")]
        public string? Name { get; set; }

        [Column("Company")]
        public string? Company { get; set; }

        [Column("User_Id")]
        [ForeignKey("User")]
        public int? UserId { get; set; }

        public User? User { get; set; }

        [Column("Event_Id")]
        [ForeignKey("Event")]
        public int EventId { get; set; }

        public Event Event { get; set; }

        public bool? IsFirstTimeEmail { get; set; } = null;
    }
}

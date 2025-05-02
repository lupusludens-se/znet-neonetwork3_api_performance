using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("Event_Occurrence")]
    public class EventOccurrence : BaseIdEntity
    {
        [Column("Event_Occurrence_Id")]
        public override int Id { get; set; }

        [Column("From_Date")]
        public DateTime FromDate { get; set; }

        [Column("To_Date")]
        public DateTime ToDate { get; set; }

        [Column("Event_Id")]
        [ForeignKey("Event")]
        public int EventId { get; set; }

        public Event Event { get; set; }
    }
}

using SE.Neo.Common.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("Event_Link")]
    public class EventLink : BaseIdEntity
    {
        [Column("Event_Link_Id")]
        public override int Id { get; set; }

        [Column("Url")]
        public string Url { get; set; }

        [Column("Name")]
        public string? Name { get; set; }

        [Column("Type")]
        public EventLinkType Type { get; set; }

        [Column("Event_Id")]
        [ForeignKey("Event")]
        public int EventId { get; set; }

        public Event Event { get; set; }
    }
}

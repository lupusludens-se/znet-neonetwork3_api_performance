using Microsoft.EntityFrameworkCore;
using SE.Neo.Core.Entities.CMS;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("Event_Invited_Region")]
    [Index(nameof(EventId), nameof(RegionId), IsUnique = true)]
    public class EventInvitedRegion : BaseIdEntity
    {
        [Column("Event_Invited_Region_Id")]
        public override int Id { get; set; }

        [Column("Region_Id")]
        [ForeignKey("Region")]
        public int RegionId { get; set; }

        public Region Region { get; set; }

        [Column("Event_Id")]
        [ForeignKey("Event")]
        public int EventId { get; set; }

        public Event Event { get; set; }
    }
}

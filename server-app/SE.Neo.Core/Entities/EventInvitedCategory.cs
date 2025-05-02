using Microsoft.EntityFrameworkCore;
using SE.Neo.Core.Entities.CMS;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("Event_Invited_Category")]
    [Index(nameof(EventId), nameof(CategoryId), IsUnique = true)]
    public class EventInvitedCategory : BaseIdEntity
    {
        [Column("Event_Invited_Category_Id")]
        public override int Id { get; set; }

        [Column("Category_Id")]
        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        public Category Category { get; set; }

        [Column("Event_Id")]
        [ForeignKey("Event")]
        public int EventId { get; set; }

        public Event Event { get; set; }
    }
}

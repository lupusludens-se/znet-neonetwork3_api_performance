using Microsoft.EntityFrameworkCore;
using SE.Neo.Core.Entities.CMS;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("Discussion_Region")]
    [Index(nameof(DisscussionId), nameof(RegionId), IsUnique = true)]
    public class DiscussionRegion : BaseIdEntity
    {
        [Column("Discussion_Region_Id")]
        public override int Id { get; set; }

        [Column("Disscussion_Id")]
        [ForeignKey("Discussion")]
        public int DisscussionId { get; set; }

        [Column("Region_Id")]
        [ForeignKey("Region")]
        public int RegionId { get; set; }

        public Region Region { get; set; }

        public Discussion Discussion { get; set; }
    }
}

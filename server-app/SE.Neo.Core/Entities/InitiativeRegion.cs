using SE.Neo.Core.Entities.CMS;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("Initiative_Region")]
    public class InitiativeRegion : BaseIdEntity
    {
        [Column("Initiative_Region_Id")]
        public override int Id { get; set; }

        [Column("Initiative_Id")]
        [ForeignKey("Initiative")]
        public int InitiativeId { get; set; }

        public Initiative Initiative { get; set; }

        [Column("CMS_Region_Id")]
        [ForeignKey("Region")]
        public int RegionId { get; set; }

        public Region Region { get; set; }
    }
}

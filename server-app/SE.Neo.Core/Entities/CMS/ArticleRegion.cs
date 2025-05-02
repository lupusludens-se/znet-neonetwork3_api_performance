using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities.CMS
{
    [Table("CMS_Article_Region")]
    public class ArticleRegion : BaseEntity
    {
        [Column("CMS_Article_Region_Id")]
        public int Id { get; set; }

        [Column("CMS_Article_Id")]
        [ForeignKey("Article")]
        public int ArticleId { get; set; }

        [Column("CMS_Region_Id")]
        [ForeignKey("Region")]
        public int RegionId { get; set; }

        public Article Article { get; set; }

        public Region Region { get; set; }
    }
}
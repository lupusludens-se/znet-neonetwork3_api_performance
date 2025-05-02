using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities.CMS
{
    [Table("CMS_Article_Technology")]
    public class ArticleTechnology : BaseEntity
    {
        [Column("CMS_Article_Technology_Id")]
        public int Id { get; set; }

        [Column("CMS_Article_Id")]
        [ForeignKey("Article")]
        public int ArticleId { get; set; }

        [Column("CMS_Technology_Id")]
        [ForeignKey("Technology")]
        public int TechnologyId { get; set; }

        public Article Article { get; set; }

        public Technology Technology { get; set; }
    }
}
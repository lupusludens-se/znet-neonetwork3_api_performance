using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities.CMS
{
    [Table("CMS_Article_Category")]
    public class ArticleCategory : BaseEntity
    {
        [Column("CMS_Article_Category_Id")]
        public int Id { get; set; }

        [Column("CMS_Article_Id")]
        [ForeignKey("Article")]
        public int ArticleId { get; set; }

        [Column("CMS_Category_Id")]
        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        public Article Article { get; set; }

        public Category Category { get; set; }
    }
}
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities.CMS
{

    [Table("CMS_Article_Content_Tag")]
    public class ArticleContentTag : BaseEntity
    {
        [Column("CMS_Article_Content_Tag_Id")]
        public int Id { get; set; }

        [Column("CMS_Article_Id")]
        [ForeignKey("Article")]
        public int ArticleId { get; set; }

        public Article Article { get; set; }

        [Column("CMS_Content_Tag_Id")]
        [ForeignKey("Content_Tag")]
        public int ContentTagId { get; set; }

        public ContentTag ContentTag { get; set; }
    }
}

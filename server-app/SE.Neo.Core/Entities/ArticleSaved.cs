using SE.Neo.Core.Entities.CMS;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("Article_Saved")]
    public class ArticleSaved : BaseIdEntity
    {
        [Column("Article_Saved_Id")]
        public override int Id { get; set; }

        [Column("Article_CMS_Id")]
        [ForeignKey("Article")]
        public int ArticleId { get; set; }

        public Article Article { get; set; }

        [Column("User_Id")]
        [ForeignKey("User")]
        public int UserId { get; set; }

        public User User { get; set; }
    }
}
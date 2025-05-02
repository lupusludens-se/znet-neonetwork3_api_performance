using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities.CMS
{
    [Table("Article_View")]
    public class ArticleView : BaseEntity

    {
        [Column("Article_View_Id")]
        public int Id { get; set; }

        [Column("User_Id")]
        [ForeignKey("User")]
        public int UserId { get; set; }

        [Column("Article_Id")]
        [ForeignKey("Article")]
        public int ArticleId { get; set; }

        public Article Article { get; set; }

        public User User { get; set; }
    }
}
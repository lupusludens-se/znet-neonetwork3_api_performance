using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities.CMS
{
    [Table("CMS_Article_Solution")]
    public class ArticleSolution : BaseEntity
    {
        [Column("CMS_Article_Solution_Id")]
        public int Id { get; set; }

        [Column("CMS_Article_Id")]
        [ForeignKey("Article")]
        public int ArticleId { get; set; }

        [Column("CMS_Solution_Id")]
        [ForeignKey("Solution")]
        public int SolutionId { get; set; }

        public Article Article { get; set; }

        public Solution Solution { get; set; }
    }
}
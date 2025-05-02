using SE.Neo.Core.Entities.CMS;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("Initiative_Article")]
    public class InitiativeArticle : BaseIdEntity
    {
        [Column("Initiative_Article_Id")]
        public override int Id { get; set; }

        [Column("Initiative_Id")]
        [ForeignKey("Initiative")]
        public int InitiativeId { get; set; }
        public Initiative Initiative { get; set; }

        [Column("Article_Id")]
        [ForeignKey("Article")]
        public int ArticleId { get; set; }

        public Article Article { get; set; }
    }
}

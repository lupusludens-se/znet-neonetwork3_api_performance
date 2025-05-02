using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities.CMS
{
    [Table("CMS_Article_Type")]
    public class ArticleType : BaseEntity
    {
        [Column("Article_Type_Id")]
        public Enums.ArticleType Id { get; set; }

        [Column("Article_Type_Name")]
        [MaxLength(250)]
        public string Name { get; set; }
    }
}
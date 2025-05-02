using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities.CMS
{
    [Table("CMS_Article_Role")]
    public class ArticleRole : BaseIdEntity
    {
        [Column("CMS_Article_Role_Id")]
        public override int Id { get; set; }

        [Column("CMS_Article_Id")]
        [ForeignKey("Article")]
        public int ArticleId { get; set; }

        [Column("Role_Id")]
        [ForeignKey("Role")]
        public int RoleId { get; set; }

        public Article Article { get; set; }

        public Role Role { get; set; }
    }
}
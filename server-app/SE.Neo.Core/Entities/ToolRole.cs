using System.ComponentModel.DataAnnotations.Schema;
namespace SE.Neo.Core.Entities
{
    [Table("Tool_Role")]
    public class ToolRole : BaseIdEntity
    {
        [Column("Tool_Role_Id")]
        public override int Id { get; set; }

        [Column("Tool_Id")]
        [ForeignKey("Tool")]
        public int ToolId { get; set; }

        [Column("Role_Id")]
        [ForeignKey("Role")]
        public int RoleId { get; set; }

        public Tool Tool { get; set; }

        public Role Role { get; set; }


    }
}

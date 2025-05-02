using SE.Neo.Core.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("Role_Permission")]
    public class RolePermission : BaseIdEntity
    {
        [Column("Role_Permission_Id")]
        public override int Id { get; set; }

        [Column("Role_Id")]
        [ForeignKey("Role")]
        public int RoleId { get; set; }

        [Column("Permission_Id")]
        [ForeignKey("Permission")]
        public PermissionType PermissionId { get; set; }

        public Role Role { get; set; }
        public Permission Permission { get; set; }
    }
}
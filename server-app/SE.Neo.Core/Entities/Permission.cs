using SE.Neo.Core.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("Permission")]
    public class Permission : BaseIdEntity
    {
        [Column("Permission_Id")]
        public PermissionType Id { get; set; }

        [Column("Permission_Name")]
        [MaxLength(250)]
        public string Name { get; set; }

        public ICollection<UserPermission> Users { get; set; }
        public ICollection<RolePermission> Roles { get; set; }
    }
}
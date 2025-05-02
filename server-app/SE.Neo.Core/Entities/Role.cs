using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("Role")]
    public class Role : BaseIdEntity
    {
        [Column("Role_Id")]
        public override int Id { get; set; }

        [Column("Role_Name")]
        [MaxLength(250)]
        public string Name { get; set; }

        [Column("Is_Special")]
        public bool IsSpecial { get; set; }

        [Column("CMS_Role_Id")]
        public int? CMSRoleId { get; set; }


        [Column("ToolTip")]
        public string? ToolTip { get; set; }

        public ICollection<UserRole> Users { get; set; }

        public ICollection<RolePermission> Permissions { get; set; }
    }
}
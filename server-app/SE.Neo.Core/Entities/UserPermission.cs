using SE.Neo.Core.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("User_Permission")]
    public class UserPermission : BaseIdEntity
    {
        [Column("User_Permission_Id")]
        public override int Id { get; set; }

        [Column("User_Id")]
        [ForeignKey("User")]
        public int UserId { get; set; }

        [Column("Permission_Id")]
        [ForeignKey("Permission")]
        public PermissionType PermissionId { get; set; }

        public User User { get; set; }
        public Permission Permission { get; set; }
    }
}
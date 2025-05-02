using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("User_Role")]
    public class UserRole : BaseIdEntity
    {
        [Column("User_Role_Id")]
        public override int Id { get; set; }

        [Column("User_Id")]
        [ForeignKey("User")]
        public int UserId { get; set; }

        [Column("Role_Id")]
        [ForeignKey("Role")]
        public int RoleId { get; set; }

        public User User { get; set; }

        public Role Role { get; set; }
    }
}
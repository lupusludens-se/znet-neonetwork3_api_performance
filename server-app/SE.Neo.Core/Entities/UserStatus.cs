using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("User_Status")]
    public class UserStatus : BaseEntity
    {
        [Column("User_Status_Id")]
        public Enums.UserStatus Id { get; set; }

        [Column("User_Status_Name")]
        [MaxLength(250)]
        public string Name { get; set; }
    }
}

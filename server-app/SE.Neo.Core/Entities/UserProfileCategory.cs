using SE.Neo.Core.Entities.CMS;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("User_Profile_Category")]
    public class UserProfileCategory : BaseIdEntity
    {
        [Column("User_Profile_Category_Id")]
        public override int Id { get; set; }

        [Column("User_Profile_Id")]
        [ForeignKey("UserProfile")]
        public int UserProfileId { get; set; }

        [Column("Category_Id")]
        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        public UserProfile UserProfile { get; set; }
        public Category Category { get; set; }
    }
}
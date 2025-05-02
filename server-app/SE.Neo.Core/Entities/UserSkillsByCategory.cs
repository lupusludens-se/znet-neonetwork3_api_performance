using SE.Neo.Core.Entities.CMS;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("User_Skills_By_Category")]
    public class UserSkillsByCategory : BaseIdEntity
    {
        [Column("User_Profile_Category_Id")]
        public override int Id { get; set; }

        [Column("User_Profile_Id")]
        [ForeignKey("UserProfile")]
        public int UserProfileId { get; set; }

        public UserProfile UserProfile { get; set; }


        [Column("Skill_Id")]
        [ForeignKey("Skills")]
        public int SkillId { get; set; }
        public Skills Skills { get; set; }

        [Column("Category_Id")]
        [ForeignKey("Categories")]
        public int CategoryId { get; set; }
        public Category Categories { get; set; }
    }
}
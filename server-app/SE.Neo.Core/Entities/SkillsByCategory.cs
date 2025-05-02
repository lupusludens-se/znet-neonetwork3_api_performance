using SE.Neo.Core.Entities.CMS;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("Skills_By_Category")]
    public class SkillsByCategory : BaseIdEntity
    {
        [Column("Skill_Category_Id")]
        public override int Id { get; set; }

        [Column("Skill_Id")]
        [ForeignKey("Skills")]
        public int SkillId { get; set; }

        public Skills Skills { get; set; }

        [Column("Category_Id")]
        [ForeignKey("Category")]
        public int CategoryId { get; set; }

        public Category Category { get; set; }
    }
}

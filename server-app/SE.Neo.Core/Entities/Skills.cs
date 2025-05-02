using SE.Neo.Common.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("Skills")]
    public class Skills : BaseIdEntity
    {
        [Column("Skill_Id")]
        public override int Id { get; set; }

        [Column("Skill_Name")]
        public string Name { get; set; }
        public ICollection<SkillsByCategory> SkillsByCategory { get; set; }

        [Column("RoleType")]
        public RoleType RoleType { get; set; }


    }
}
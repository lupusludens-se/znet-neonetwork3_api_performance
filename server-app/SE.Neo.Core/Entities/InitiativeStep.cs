using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("Initiative_Step")]
    public class InitiativeStep : BaseIdEntity
    {
        [Column("Initiative_Step_Id")]
        public override int Id { get; set; }

        [Column("Step_Name")]
        public string Name { get; set; }

        [Column("Step_Description")]
        public string Description { get; set; }

        public ICollection<InitiativeSubStep> InitiativeSubSteps { get; set; }

    }
}

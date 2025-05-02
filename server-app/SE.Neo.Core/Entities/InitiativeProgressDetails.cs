using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("Initiative_Progress_Details")]
    public class InitiativeProgressDetails : BaseIdEntity
    {
        [Column("Initiative_Progress_Details_Id")]
        public override int Id { get; set; }


        [Column("Initiative_Id")]
        [ForeignKey("Initiative")]
        public int InitiativeId { get; set; }
        public Initiative Initiative { get; set; }

        [Column("Initiative_Step_Id")]
        [ForeignKey("InitiativeStep")]
        public int StepId { get; set; }
        public InitiativeStep Step { get; set; }

        [Column("Initiative_Sub_Step_Id")]
        [ForeignKey("InitiativeSubStep")]
        public int SubStepId { get; set; }
        public InitiativeSubStep InitiativeSubStep { get; set; }

        [Column("Is_Checked")]
        public bool IsChecked { get; set; }
    }
}

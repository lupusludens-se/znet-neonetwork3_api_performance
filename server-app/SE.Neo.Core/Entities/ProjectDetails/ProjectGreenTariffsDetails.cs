using SE.Neo.Core.Models.Project;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities.ProjectDetails
{
    [Table("Project_Green_Tariffs_Details")]
    public class ProjectGreenTariffsDetails : BaseCommentedProjectDetails
    {
        [Column("Project_Green_Tariffs_Details_Id")]
        public override int Id { get; set; }

        [Column("Utility_Name")]
        [MaxLength(100)]
        public string UtilityName { get; set; }

        [Column("Program_Website")]
        [MaxLength(2048)]
        public string ProgramWebsite { get; set; }

        [Column("Minimum_Purchase_Volume")]
        public float? MinimumPurchaseVolume { get; set; }

        [Column("Term_Length_Id")]
        [ForeignKey("TermLength")]
        public Enums.TermLengthType? TermLengthId { get; set; }

        public TermLength? TermLength { get; set; }
    }
}
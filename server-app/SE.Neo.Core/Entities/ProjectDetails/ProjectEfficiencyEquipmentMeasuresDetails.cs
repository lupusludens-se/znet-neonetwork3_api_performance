using SE.Neo.Core.Models.Project;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities.ProjectDetails
{
    [Table("Project_Efficiency_Equipment_Measures_Details")]
    public class ProjectEfficiencyEquipmentMeasuresDetails : BaseCommentedProjectDetails
    {
        [Column("Project_Efficiency_Equipment_Measures_Details_Id")]
        public override int Id { get; set; }

        [Column("Minimum_Term_Length")]
        public int? MinimumTermLength { get; set; }

        [Column("Is_Investment_Grade_Credit_Of_Offtaker_Required")]
        public bool? IsInvestmentGradeCreditOfOfftakerRequired { get; set; }
    }
}
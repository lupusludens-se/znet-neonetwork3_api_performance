using SE.Neo.Core.Models.Project;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities.ProjectDetails
{
    [Table("Project_Emerging_Technology_Details")]
    public class ProjectEmergingTechnologyDetails : BaseCommentedProjectDetails
    {
        [Column("Project_Emerging_Technology_Details_Id")]
        public override int Id { get; set; }

        [Column("Minimum_Annual_Value")]
        public float? MinimumAnnualValue { get; set; }

        [Column("Energy_Unit_Id")]
        public Enums.EnergyUnit? EnergyUnitId { get; set; }

        [Column("Minimum_Term_Length")]
        public int? MinimumTermLength { get; set; }
    }
}
using SE.Neo.Core.Models.Project;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities.ProjectDetails
{
    [Table("Project_Battery_Storage_Details")]
    public class ProjectBatteryStorageDetails : BaseCommentedProjectDetails
    {
        [Column("Project_Battery_Storage_Details_Id")]
        public override int Id { get; set; }

        [Column("Minimum_Annual")]
        public float? MinimumAnnualPeakKW { get; set; }

        [Column("Minimum_Term_Length")]
        public int? MinimumTermLength { get; set; }
    }
}
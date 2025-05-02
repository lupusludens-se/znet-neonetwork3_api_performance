using SE.Neo.Core.Models.Project;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities.ProjectDetails
{
    [Table("Project_EV_Charging_Details")]
    public class ProjectEVChargingDetails : BaseCommentedProjectDetails
    {
        [Column("Project_EV_Charging_Details_Id")]
        public override int Id { get; set; }

        [Column("Minimum_Charging_Stations_Required")]
        public int? MinimumChargingStationsRequired { get; set; }

        [Column("Minimum_Term_Length")]
        public int? MinimumTermLength { get; set; }
    }
}
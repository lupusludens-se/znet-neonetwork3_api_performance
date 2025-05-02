using SE.Neo.Core.Models.Project;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities.ProjectDetails
{
    [Table("Project_Onsite_Solar_Details")]
    public class ProjectOnsiteSolarDetails : BaseCommentedProjectDetails
    {
        [Column("Project_Onsite_Solar_Details_Id")]
        public override int Id { get; set; }

        [Column("Minimum_Annual_Site_KWh")]
        public float? MinimumAnnualSiteKWh { get; set; }

        [Column("Minimum_Term_Length")]
        public int? MinimumTermLength { get; set; }
    }
}
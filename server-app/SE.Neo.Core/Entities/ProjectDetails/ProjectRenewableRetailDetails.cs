using SE.Neo.Core.Models.Project;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities.ProjectDetails
{
    [Table("Project_Renewable_Retail_Details")]
    public class ProjectRenewableRetailDetails : BaseCommentedProjectDetails
    {
        [Column("Project_Renewable_Retail_Details_Id")]
        public override int Id { get; set; }

        [Column("Minimum_Annual_Site_KWh")]
        public float? MinimumAnnualSiteKWh { get; set; }

        public ICollection<ProjectRenewableRetailDetailsPurchaseOption> PurchaseOptions { get; set; }

        [Column("Minimum_Term_Length")]
        public int? MinimumTermLength { get; set; }
    }
}
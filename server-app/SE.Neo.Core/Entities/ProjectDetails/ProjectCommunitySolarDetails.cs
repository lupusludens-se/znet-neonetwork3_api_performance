using SE.Neo.Core.Models.Project;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities.ProjectDetails
{
    [Table("Project_Community_Solar_Details")]
    public class ProjectCommunitySolarDetails : BaseCommentedProjectDetails
    {
        [Column("Project_Community_Solar_Details_Id")]
        public override int Id { get; set; }

        [Column("Minimum_Annual_MWh")]
        public float? MinimumAnnualMWh { get; set; }

        [Column("Total_Annual_MWh")]
        public int? TotalAnnualMWh { get; set; }

        [Column("Utility_Territory")]
        [MaxLength(100)]
        public string UtilityTerritory { get; set; }

        [Column("Project_Available")]
        public bool? ProjectAvailable { get; set; }

        [Column("Project_Availability_Approximate_Date")]
        public DateTime? ProjectAvailabilityApproximateDate { get; set; }

        [Column("Is_Investment_Grade_Credit_Of_Offtaker_Required")]
        public bool? IsInvestmentGradeCreditOfOfftakerRequired { get; set; }

        [Column("Minimum_Term_Length")]
        public int? MinimumTermLength { get; set; }
    }
}
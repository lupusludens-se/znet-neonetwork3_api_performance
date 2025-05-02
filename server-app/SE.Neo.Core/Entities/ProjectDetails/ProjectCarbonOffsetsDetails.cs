using SE.Neo.Core.Models.Project;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities.ProjectDetails
{
    [Table("Project_Carbon_Offsets_Details")]
    public class ProjectCarbonOffsetsDetails : BaseCommentedProjectDetails
    {
        [Column("Project_Carbon_Offsets_Details_Id")]
        public override int Id { get; set; }

        [Column("Minimum_Purchase_Volume")]
        public float? MinimumPurchaseVolume { get; set; }

        public ICollection<ProjectCarbonOffsetsDetailsTermLength> StripLengths { get; set; }
    }
}
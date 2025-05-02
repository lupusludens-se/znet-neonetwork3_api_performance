using SE.Neo.Core.Models.Project;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities.ProjectDetails
{
    [Table("Project_EAC_Details")]
    public class ProjectEACDetails : BaseCommentedProjectDetails
    {
        [Column("Project_EAC_Details_Id")]
        public override int Id { get; set; }

        [Column("Minimum_Purchase_Volume")]
        public float? MinimumPurchaseVolume { get; set; }

        public ICollection<ProjectEACDetailsTermLength> StripLengths { get; set; }

        [Column("Minimum_Term_Length")]
        public int? MinimumTermLength { get; set; }
    }
}
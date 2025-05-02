using SE.Neo.Core.Constants;
using SE.Neo.WebAPI.Validation;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Models.Project
{
    public class ProjectCarbonOffsetsRequest : BaseProjectDetailedRequest<ProjectCarbonOffsetsDetailsRequest>
    {
        [Required]
        [AllowedProjectCategories(CategoriesSlugs.CarbonOffsets)]
        public override ProjectRequest Project { get; set; }

        [Required]
        public override ProjectCarbonOffsetsDetailsRequest ProjectDetails { get; set; }
    }
}

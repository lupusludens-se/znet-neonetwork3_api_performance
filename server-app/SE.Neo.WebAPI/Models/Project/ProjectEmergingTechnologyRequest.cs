using SE.Neo.Core.Constants;
using SE.Neo.WebAPI.Validation;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Models.Project
{
    public class ProjectEmergingTechnologyRequest : BaseProjectDetailedRequest<ProjectEmergingTechnologyDetailsRequest>
    {
        [Required]
        [AllowedProjectCategories(CategoriesSlugs.EmergingTechnology)]
        public override ProjectRequest Project { get; set; }

        [Required]
        public override ProjectEmergingTechnologyDetailsRequest ProjectDetails { get; set; }
    }
}

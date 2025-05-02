using SE.Neo.Core.Constants;
using SE.Neo.WebAPI.Validation;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Models.Project
{
    public class ProjectOnsiteSolarRequest : BaseProjectDetailedRequest<ProjectOnsiteSolarDetailsRequest>
    {
        [Required]
        [AllowedProjectCategories(CategoriesSlugs.OnsiteSolar)]
        public override ProjectRequest Project { get; set; }

        [Required]
        public override ProjectOnsiteSolarDetailsRequest ProjectDetails { get; set; }
    }
}

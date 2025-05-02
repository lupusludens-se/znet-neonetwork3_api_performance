using SE.Neo.Core.Constants;
using SE.Neo.WebAPI.Validation;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Models.Project
{
    public class ProjectCommunitySolarRequest : BaseProjectDetailedRequest<ProjectCommunitySolarDetailsRequest>
    {
        [Required]
        [AllowedProjectCategories(CategoriesSlugs.CommunitySolar)]
        public override ProjectRequest Project { get; set; }

        [Required]
        public override ProjectCommunitySolarDetailsRequest ProjectDetails { get; set; }
    }
}

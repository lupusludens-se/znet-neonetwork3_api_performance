using SE.Neo.Core.Constants;
using SE.Neo.WebAPI.Validation;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Models.Project
{
    public class ProjectEACRequest : BaseProjectDetailedRequest<ProjectEACDetailsRequest>
    {
        [Required]
        [AllowedProjectCategories(CategoriesSlugs.EAC)]
        public override ProjectRequest Project { get; set; }

        [Required]
        public override ProjectEACDetailsRequest ProjectDetails { get; set; }
    }
}

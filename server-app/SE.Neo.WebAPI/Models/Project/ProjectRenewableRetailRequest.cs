using SE.Neo.Core.Constants;
using SE.Neo.WebAPI.Validation;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Models.Project
{
    public class ProjectRenewableRetailRequest : BaseProjectDetailedRequest<ProjectRenewableRetailDetailsRequest>
    {
        [Required]
        [AllowedProjectCategories(CategoriesSlugs.RenewableRetail)]
        public override ProjectRequest Project { get; set; }

        [Required]
        public override ProjectRenewableRetailDetailsRequest ProjectDetails { get; set; }
    }
}

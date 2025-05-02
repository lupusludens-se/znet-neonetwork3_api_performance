using SE.Neo.Core.Constants;
using SE.Neo.WebAPI.Validation;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Models.Project
{
    public class ProjectGreenTariffsRequest : BaseProjectDetailedRequest<ProjectGreenTariffsDetailsRequest>
    {
        [Required]
        [AllowedProjectCategories(CategoriesSlugs.GreenTariffs)]
        public override ProjectRequest Project { get; set; }

        [Required]
        public override ProjectGreenTariffsDetailsRequest ProjectDetails { get; set; }
    }
}

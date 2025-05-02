using SE.Neo.Core.Constants;
using SE.Neo.WebAPI.Validation;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Models.Project
{
    public class ProjectEfficiencyAuditsAndConsultingRequest : BaseProjectDetailedRequest<ProjectEfficiencyAuditsAndConsultingDetailsRequest>
    {
        [Required]
        [AllowedProjectCategories(CategoriesSlugs.EfficiencyAuditsAndConsulting)]
        public override ProjectRequest Project { get; set; }

        [Required]
        public override ProjectEfficiencyAuditsAndConsultingDetailsRequest ProjectDetails { get; set; }
    }
}

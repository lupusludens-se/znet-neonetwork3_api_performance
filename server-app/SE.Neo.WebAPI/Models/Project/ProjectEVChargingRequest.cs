using SE.Neo.Core.Constants;
using SE.Neo.WebAPI.Validation;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Models.Project
{
    public class ProjectEVChargingRequest : BaseProjectDetailedRequest<ProjectEVChargingDetailsRequest>
    {
        [Required]
        [AllowedProjectCategories(CategoriesSlugs.EVCharging)]
        public override ProjectRequest Project { get; set; }

        [Required]
        public override ProjectEVChargingDetailsRequest ProjectDetails { get; set; }
    }
}

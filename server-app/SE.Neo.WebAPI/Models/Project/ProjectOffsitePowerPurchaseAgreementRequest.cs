using SE.Neo.Core.Constants;
using SE.Neo.WebAPI.Validation;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Models.Project
{
    public class ProjectOffsitePowerPurchaseAgreementRequest : BaseProjectDetailedRequest<ProjectOffsitePowerPurchaseAgreementDetailsRequest>
    {
        [Required]
        [AllowedProjectCategories(CategoriesSlugs.OffsitePowerPurchaseAgreement, CategoriesSlugs.AggregatedPowerPurchaseAgreement)]
        public override ProjectRequest Project { get; set; }

        [Required]
        public override ProjectOffsitePowerPurchaseAgreementDetailsRequest ProjectDetails { get; set; }
    }
}

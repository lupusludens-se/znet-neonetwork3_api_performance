using SE.Neo.Core.Enums;
using SE.Neo.WebAPI.Enums;
using SE.Neo.WebAPI.Models.Shared;
using SE.Neo.WebAPI.Validation;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Models.Project
{
    public class ProjectRenewableRetailDetailsRequest : BaseCommentedProjectDetailsRequest
    {
        [RequiredIfNot(nameof(StatusId), ProjectStatus.Draft), Range(1, float.MaxValue)]
        public float? MinimumAnnualSiteKWh { get; set; }

        [Range(1, int.MaxValue)]
        public int? MinimumTermLength { get; set; }

        [EnumRequestEnumerableExist(typeof(PurchaseOptionType), "must contain valid purchase option type id")]
        public IEnumerable<EnumRequest<PurchaseOptionType>>? PurchaseOptions { get; set; }

        [EnumRequestEnumerableExist(typeof(ProjectRenewableRetailValueProvidedType), "must contain valid renewable retail project value provided type id")]
        public IEnumerable<EnumRequest<ProjectRenewableRetailValueProvidedType>>? ValuesProvided { get; set; }
    }
}

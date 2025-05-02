using SE.Neo.Core.Enums;
using SE.Neo.WebAPI.Enums;
using SE.Neo.WebAPI.Models.Shared;
using SE.Neo.WebAPI.Validation;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Models.Project
{
    public class ProjectCommunitySolarDetailsRequest : BaseCommentedProjectDetailsRequest
    {
        [RequiredIfNot(nameof(StatusId), ProjectStatus.Draft), Range(1, float.MaxValue)]
        public float? MinimumAnnualMWh { get; set; }

        [RequiredIfNot(nameof(StatusId), ProjectStatus.Draft), Range(1, int.MaxValue)]
        public int? TotalAnnualMWh { get; set; }

        [RequiredIfNot(nameof(StatusId), ProjectStatus.Draft)]
        [StringLength(100)]
        public string? UtilityTerritory { get; set; }

        [RequiredIfNot(nameof(StatusId), ProjectStatus.Draft)]
        public bool? ProjectAvailable { get; set; }

        public DateTime? ProjectAvailabilityApproximateDate { get; set; }

        [RequiredIfNot(nameof(StatusId), ProjectStatus.Draft)]
        public bool? IsInvestmentGradeCreditOfOfftakerRequired { get; set; }

        [EnumRequestEnumerableExist(typeof(ProjectCommunitySolarContractStructureType), "must contain valid community solar project contract structure type id")]
        public IEnumerable<EnumRequest<ProjectCommunitySolarContractStructureType>>? ContractStructures { get; set; }

        [Range(1, int.MaxValue)]
        public int? MinimumTermLength { get; set; }

        [EnumRequestEnumerableExist(typeof(ProjectCommunitySolarValueProvidedType), "must contain valid community solar project value provided type id")]
        public IEnumerable<EnumRequest<ProjectCommunitySolarValueProvidedType>>? ValuesProvided { get; set; }
    }
}

using SE.Neo.Core.Enums;
using SE.Neo.WebAPI.Enums;
using SE.Neo.WebAPI.Models.Shared;
using SE.Neo.WebAPI.Validation;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Models.Project
{
    public class ProjectEfficiencyAuditsAndConsultingDetailsRequest : BaseCommentedProjectDetailsRequest
    {
        [EnumRequestEnumerableExist(typeof(ProjectEfficiencyAuditsAndConsultingContractStructureType), "must contain valid efficiency audits and consulting project contract structure type id")]
        public IEnumerable<EnumRequest<ProjectEfficiencyAuditsAndConsultingContractStructureType>>? ContractStructures { get; set; }

        [Range(1, int.MaxValue)]
        public int? MinimumTermLength { get; set; }

        [RequiredIfNot(nameof(StatusId), ProjectStatus.Draft)]
        public bool? IsInvestmentGradeCreditOfOfftakerRequired { get; set; }

        [EnumRequestEnumerableExist(typeof(ProjectEfficiencyAuditsAndConsultingValueProvidedType), "must contain valid efficiency audits and consulting project value provided type id")]
        public IEnumerable<EnumRequest<ProjectEfficiencyAuditsAndConsultingValueProvidedType>>? ValuesProvided { get; set; }
    }
}

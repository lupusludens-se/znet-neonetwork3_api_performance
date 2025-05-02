using SE.Neo.Core.Enums;
using SE.Neo.WebAPI.Enums;
using SE.Neo.WebAPI.Models.Shared;
using SE.Neo.WebAPI.Validation;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Models.Project
{
    public class ProjectEfficiencyEquipmentMeasuresDetailsRequest : BaseCommentedProjectDetailsRequest
    {
        [EnumRequestEnumerableExist(typeof(ProjectEfficiencyEquipmentMeasuresContractStructureType), "must contain valid efficiency equipment measures project contract structure type id")]
        public IEnumerable<EnumRequest<ProjectEfficiencyEquipmentMeasuresContractStructureType>>? ContractStructures { get; set; }

        [Range(1, int.MaxValue)]
        public int? MinimumTermLength { get; set; }

        [RequiredIfNot(nameof(StatusId), ProjectStatus.Draft)]
        public bool? IsInvestmentGradeCreditOfOfftakerRequired { get; set; }

        [EnumRequestEnumerableExist(typeof(ProjectEfficiencyEquipmentMeasuresValueProvidedType), "must contain valid efficiency equipment measures project value provided type id")]
        public IEnumerable<EnumRequest<ProjectEfficiencyEquipmentMeasuresValueProvidedType>>? ValuesProvided { get; set; }
    }
}

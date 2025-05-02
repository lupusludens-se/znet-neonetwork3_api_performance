using SE.Neo.Core.Enums;
using SE.Neo.WebAPI.Enums;
using SE.Neo.WebAPI.Models.Shared;
using SE.Neo.WebAPI.Validation;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Models.Project
{
    public class ProjectEmergingTechnologyDetailsRequest : BaseCommentedProjectDetailsRequest
    {
        [RequiredIfNot(nameof(StatusId), ProjectStatus.Draft), Range(1, float.MaxValue)]
        public float? MinimumAnnualValue { get; set; }

        [EnumExist(typeof(EnergyUnit), "must contain valid energy unit id")]
        public EnergyUnit? EnergyUnitId { get; set; }

        [EnumExist(typeof(ProjectEmergingTechnologyContractStructureType), "must contain valid emerging technology project contract structure type id")]
        public IEnumerable<EnumRequest<ProjectEmergingTechnologyContractStructureType>>? ContractStructures { get; set; }

        [Range(1, int.MaxValue)]
        public int? MinimumTermLength { get; set; }

        [EnumExist(typeof(ProjectEmergingTechnologyValueProvidedType), "must contain valid emerging technology project value provided type id")]
        public IEnumerable<EnumRequest<ProjectEmergingTechnologyValueProvidedType>>? ValuesProvided { get; set; }
    }
}

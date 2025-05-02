using SE.Neo.Core.Enums;
using SE.Neo.WebAPI.Enums;
using SE.Neo.WebAPI.Models.Shared;
using SE.Neo.WebAPI.Validation;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Models.Project
{
    public class ProjectBatteryStorageDetailsRequest : BaseCommentedProjectDetailsRequest
    {
        [RequiredIfNot(nameof(StatusId), ProjectStatus.Draft), Range(1, float.MaxValue)]
        public float? MinimumAnnualPeakKW { get; set; }

        [EnumRequestEnumerableExist(typeof(ProjectBatteryStorageContractStructureType), "must contain valid battery storage project contract structure type id")]
        public IEnumerable<EnumRequest<ProjectBatteryStorageContractStructureType>>? ContractStructures { get; set; }

        [Range(1, int.MaxValue)]
        public int? MinimumTermLength { get; set; }

        [EnumRequestEnumerableExist(typeof(ProjectBatteryStorageValueProvidedType), "must contain valid battery storage project value provided type id")]
        public IEnumerable<EnumRequest<ProjectBatteryStorageValueProvidedType>>? ValuesProvided { get; set; }
    }
}

using SE.Neo.Core.Enums;
using SE.Neo.WebAPI.Enums;
using SE.Neo.WebAPI.Models.Shared;
using SE.Neo.WebAPI.Validation;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Models.Project
{
    public class ProjectEVChargingDetailsRequest : BaseCommentedProjectDetailsRequest
    {
        [RequiredIfNot(nameof(StatusId), ProjectStatus.Draft), Range(1, int.MaxValue)]
        public int? MinimumChargingStationsRequired { get; set; }

        [EnumRequestEnumerableExist(typeof(ProjectEVChargingContractStructureType), "must contain valid EV charging project contract structure type id")]
        public IEnumerable<EnumRequest<ProjectEVChargingContractStructureType>>? ContractStructures { get; set; }

        [Range(1, int.MaxValue)]
        public int? MinimumTermLength { get; set; }

        [EnumRequestEnumerableExist(typeof(ProjectEVChargingValueProvidedType), "must contain valid EV charging project value provided type id")]
        public IEnumerable<EnumRequest<ProjectEVChargingValueProvidedType>>? ValuesProvided { get; set; }
    }
}

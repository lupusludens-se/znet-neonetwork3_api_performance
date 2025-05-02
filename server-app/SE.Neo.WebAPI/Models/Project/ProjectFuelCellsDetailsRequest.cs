using SE.Neo.Core.Enums;
using SE.Neo.WebAPI.Enums;
using SE.Neo.WebAPI.Models.Shared;
using SE.Neo.WebAPI.Validation;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Models.Project
{
    public class ProjectFuelCellsDetailsRequest : BaseCommentedProjectDetailsRequest
    {
        [RequiredIfNot(nameof(StatusId), ProjectStatus.Draft), Range(1, float.MaxValue)]
        public float? MinimumAnnualSiteKWh { get; set; }

        [EnumRequestEnumerableExist(typeof(ProjectFuelCellsContractStructureType), "must contain valid fuel cells project contract structure type id")]
        public IEnumerable<EnumRequest<ProjectFuelCellsContractStructureType>>? ContractStructures { get; set; }

        [Range(1, int.MaxValue)]
        public int? MinimumTermLength { get; set; }

        [EnumRequestEnumerableExist(typeof(ProjectFuelCellsValueProvidedType), "must contain valid fuel cells project value provided type id")]
        public IEnumerable<EnumRequest<ProjectFuelCellsValueProvidedType>>? ValuesProvided { get; set; }
    }
}

using SE.Neo.Core.Enums;
using SE.Neo.WebAPI.Enums;
using SE.Neo.WebAPI.Models.Shared;
using SE.Neo.WebAPI.Validation;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Models.Project
{
    public class ProjectOnsiteSolarDetailsRequest : BaseCommentedProjectDetailsRequest
    {
        [RequiredIfNot(nameof(StatusId), ProjectStatus.Draft), Range(1, float.MaxValue)]
        public float? MinimumAnnualSiteKWh { get; set; }

        [EnumRequestEnumerableExist(typeof(ProjectOnsiteSolarContractStructureType), "must contain valid onsite solar project contract structure type id")]
        public IEnumerable<EnumRequest<ProjectOnsiteSolarContractStructureType>>? ContractStructures { get; set; }

        [Range(1, int.MaxValue)]
        public int? MinimumTermLength { get; set; }

        [EnumRequestEnumerableExist(typeof(ProjectOnsiteSolarValueProvidedType), "must contain valid onsite solar project value provided type id")]
        public IEnumerable<EnumRequest<ProjectOnsiteSolarValueProvidedType>>? ValuesProvided { get; set; }
    }
}

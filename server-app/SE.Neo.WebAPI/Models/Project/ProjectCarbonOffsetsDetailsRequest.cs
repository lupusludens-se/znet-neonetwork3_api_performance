using SE.Neo.Core.Enums;
using SE.Neo.WebAPI.Enums;
using SE.Neo.WebAPI.Models.Shared;
using SE.Neo.WebAPI.Validation;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Models.Project
{
    public class ProjectCarbonOffsetsDetailsRequest : BaseCommentedProjectDetailsRequest
    {
        [RequiredIfNot(nameof(StatusId), ProjectStatus.Draft), Range(1, float.MaxValue)]
        public float? MinimumPurchaseVolume { get; set; }

        [EnumRequestEnumerableExist(typeof(TermLengthType), "must contain valid strip lengths ids")]
        public IEnumerable<EnumRequest<TermLengthType>>? StripLengths { get; set; }

        [EnumRequestEnumerableExist(typeof(ProjectCarbonOffsetsValueProvidedType), "must contain valid carbon offsets project value provided type id")]
        public IEnumerable<EnumRequest<ProjectCarbonOffsetsValueProvidedType>>? ValuesProvided { get; set; }
    }
}

using SE.Neo.Core.Enums;
using SE.Neo.WebAPI.Enums;
using SE.Neo.WebAPI.Models.Shared;
using SE.Neo.WebAPI.Validation;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Models.Project
{
    public class ProjectEACDetailsRequest : BaseCommentedProjectDetailsRequest
    {
        [RequiredIfNot(nameof(StatusId), ProjectStatus.Draft), Range(1, float.MaxValue)]
        public float? MinimumPurchaseVolume { get; set; }

        [EnumRequestEnumerableExist(typeof(TermLengthType), "must contain valid strip lenght type id")]
        public IEnumerable<EnumRequest<TermLengthType>>? StripLengths { get; set; }

        [Range(1, int.MaxValue)]
        public int? MinimumTermLength { get; set; }

        [EnumRequestEnumerableExist(typeof(ProjectEACValueProvidedType), "must contain valid EAC project value provided type id")]
        public IEnumerable<EnumRequest<ProjectEACValueProvidedType>>? ValuesProvided { get; set; }
    }
}

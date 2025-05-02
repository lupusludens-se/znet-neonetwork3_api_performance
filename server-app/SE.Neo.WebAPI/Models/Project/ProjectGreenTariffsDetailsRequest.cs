using SE.Neo.Core.Enums;
using SE.Neo.WebAPI.Enums;
using SE.Neo.WebAPI.Models.Shared;
using SE.Neo.WebAPI.Validation;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Models.Project
{
    public class ProjectGreenTariffsDetailsRequest : BaseCommentedProjectDetailsRequest
    {
        [RequiredIfNot(nameof(StatusId), ProjectStatus.Draft)]
        [StringLength(100)]
        public string? UtilityName { get; set; }

        [RequiredIfNot(nameof(StatusId), ProjectStatus.Draft)]
        [UrlCustom]
        [StringLength(2048)]
        public string? ProgramWebsite { get; set; }

        [RequiredIfNot(nameof(StatusId), ProjectStatus.Draft), Range(1, float.MaxValue)]
        public float? MinimumPurchaseVolume { get; set; }

        [RequiredIfNot(nameof(StatusId), ProjectStatus.Draft)]
        [EnumExist(typeof(TermLengthType), "must contain valid term length type id")]
        public TermLengthType? TermLengthId { get; set; }

        [EnumRequestEnumerableExist(typeof(ProjectGreenTariffsValueProvidedType), "must contain valid green tariffs project value provided type id")]
        public IEnumerable<EnumRequest<ProjectGreenTariffsValueProvidedType>>? ValuesProvided { get; set; }
    }
}

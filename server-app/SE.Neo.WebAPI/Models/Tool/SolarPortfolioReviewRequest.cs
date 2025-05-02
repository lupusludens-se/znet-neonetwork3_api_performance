using SE.Neo.WebAPI.Enums;
using SE.Neo.WebAPI.Validation;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Models.Tool
{
    public class SolarPortfolioReviewRequest
    {
        [NonEmptyList, EnumsEnumerableExist(typeof(QuoteValueProvidedType), "must contain valid quote interest ids")]
        public IEnumerable<QuoteValueProvidedType>? Interests { get; set; }

        [MaxLength(4000)]
        public string? AdditionalComments { get; set; }
    }
}

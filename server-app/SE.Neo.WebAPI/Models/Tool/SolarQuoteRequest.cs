using SE.Neo.Common.Enums;
using SE.Neo.WebAPI.Enums;
using SE.Neo.WebAPI.Validation;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Models.Tool
{
    public class SolarQuoteRequest
    {
        [NonEmptyList, EnumsEnumerableExist(typeof(QuoteValueProvidedType), "must contain valid quote interest ids")]
        public IEnumerable<QuoteValueProvidedType> Interests { get; set; }

        [NonEmptyList, EnumsEnumerableExist(typeof(QuoteContractStructureType), "must contain valid quote contract structure ids")]
        public IEnumerable<QuoteContractStructureType> ContractStructures { get; set; }

        [MaxLength(4000)]
        public string? AdditionalComments { get; set; }

        [NotNullOrWhiteSpace, MaxLength(250)]
        public string SiteAddress { get; set; }

        [Required, Range(1, long.MaxValue)]
        public long AnnualPower { get; set; }

        [Required]
        public bool BuildingOwned { get; set; }

        [Required]
        public bool RoofAvailable { get; set; }

        [RequiredIf(nameof(RoofAvailable), true)]
        [Range(1, long.MaxValue)]
        public long? RoofArea { get; set; }

        [RequiredIf(nameof(RoofAvailable), true), EnumExist(typeof(AreaType), "must contain valid area type id")]
        public AreaType? RoofAreaType { get; set; }

        [Required]
        public bool LandAvailable { get; set; }

        [RequiredIf(nameof(LandAvailable), true), Range(1, long.MaxValue)]
        public long? LandArea { get; set; }

        [RequiredIf(nameof(LandAvailable), true), EnumExist(typeof(AreaType), "must contain valid area type id")]
        public AreaType? LandAreaType { get; set; }

        [Required]
        public bool CarportAvailable { get; set; }

        [RequiredIf(nameof(CarportAvailable), true), Range(1, long.MaxValue)]
        public long? CarportArea { get; set; }

        [RequiredIf(nameof(CarportAvailable), true), EnumExist(typeof(AreaType), "must contain valid area type id")]
        public AreaType? CarportAreaType { get; set; }

    }
}

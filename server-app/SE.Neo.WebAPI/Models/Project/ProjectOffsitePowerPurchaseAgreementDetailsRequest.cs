using SE.Neo.Core.Enums;
using SE.Neo.WebAPI.Enums;
using SE.Neo.WebAPI.Models.Shared;
using SE.Neo.WebAPI.Validation;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Models.Project
{
    public class ProjectOffsitePowerPurchaseAgreementDetailsRequest : BaseProjectDetailsRequest
    {
        [RequiredIfNot(nameof(StatusId), ProjectStatus.Draft)]
        public double? Latitude { get; set; }

        [RequiredIfNot(nameof(StatusId), ProjectStatus.Draft)]
        public double? Longitude { get; set; }

        [EnumExist(typeof(IsoRtoType), "must contain valid ISO\\RTO type id")]
        public IsoRtoType? IsoRtoId { get; set; }

        [RequiredIfNot(nameof(StatusId), ProjectStatus.Draft)]
        [EnumExist(typeof(ProductType), "must contain valid product type id")]
        public ProductType? ProductTypeId { get; set; }

        [RequiredIfNot(nameof(StatusId), ProjectStatus.Draft)]
        public DateTime? CommercialOperationDate { get; set; }

        [EnumRequestEnumerableExist(typeof(ProjectOffsitePPAValueProvidedType), "must contain valid values to offtakers types id")]
        public IEnumerable<EnumRequest<ProjectOffsitePPAValueProvidedType>>? ValuesToOfftakers { get; set; }

        [RequiredIfNot(nameof(StatusId), ProjectStatus.Draft), Range(1, int.MaxValue)]
        public int? PPATermYearsLength { get; set; }

        [RequiredIfNot(nameof(StatusId), ProjectStatus.Draft), Range(1, int.MaxValue)]
        public int? TotalProjectNameplateMWACCapacity { get; set; }

        [RequiredIfNot(nameof(StatusId), ProjectStatus.Draft), Range(1, int.MaxValue)]
        public int? TotalProjectExpectedAnnualMWhProductionP50 { get; set; }

        [RequiredIfNot(nameof(StatusId), ProjectStatus.Draft), Range(1, float.MaxValue)]
        public float? MinimumOfftakeMWhVolumeRequired { get; set; }

        [StringLength(500)]
        public string? NotesForPotentialOfftakers { get; set; }

        [EnumExist(typeof(SettlementType), "must contain valid settlement type id")]
        public SettlementType? SettlementTypeId { get; set; }

        [EnumExist(typeof(SettlementHubOrLoadZoneType), "must contain valid settlement hub or loadzone type id")]
        public SettlementHubOrLoadZoneType? SettlementHubOrLoadZoneId { get; set; }

        [RequiredIfNot(nameof(StatusId), ProjectStatus.Draft)]
        [EnumExist(typeof(Currency), "must contain valid currency type id")]
        public Currency? ForAllPriceEntriesCurrencyId { get; set; }

        [RequiredIfNot(nameof(StatusId), ProjectStatus.Draft), StringLength(100)]
        public string? ContractPricePerMWh { get; set; }

        [StringLength(100)]
        public string? FloatingMarketSwapIndexDiscount { get; set; }

        [StringLength(100)]
        public string? FloatingMarketSwapFloor { get; set; }

        [StringLength(100)]
        public string? FloatingMarketSwapCap { get; set; }

        [RequiredIfNot(nameof(StatusId), ProjectStatus.Draft)]
        [EnumExist(typeof(PricingStructureType), "must contain valid pricing structure type id")]
        public PricingStructureType? PricingStructureId { get; set; }

        [Range(0, 100)]
        [SumBelowEquals(100, nameof(UpsidePercentageToOfftaker))]
        public int? UpsidePercentageToDeveloper { get; set; }

        [Range(0, 100)]
        [SumBelowEquals(100, nameof(UpsidePercentageToDeveloper))]
        public int? UpsidePercentageToOfftaker { get; set; }

        [StringLength(100)]
        public string? DiscountAmount { get; set; }

        [RequiredIfNot(nameof(StatusId), ProjectStatus.Draft)]
        [EnumExist(typeof(EACType), "must contain valid EAC type id")]
        public EACType? EACId { get; set; }

        [StringLength(100)]
        public string? EACCustom { get; set; }

        [StringLength(100)]
        public string? EACValue { get; set; }

        [RequiredIfNot(nameof(StatusId), ProjectStatus.Draft)]
        [EnumExist(typeof(SettlementPriceIntervalType), "must contain valid settlement price interval type id")]
        public SettlementPriceIntervalType? SettlementPriceIntervalId { get; set; }

        [StringLength(100)]
        public string? SettlementPriceIntervalCustom { get; set; }

        [RequiredIfNot(nameof(StatusId), ProjectStatus.Draft)]
        [EnumExist(typeof(SettlementCalculationIntervalType), "must contain valid settlement calculation interval type id")]
        public SettlementCalculationIntervalType? SettlementCalculationIntervalId { get; set; }

        [StringLength(500)]
        public string? AdditionalNotesForSEOperationsTeam { get; set; }

        [RequiredIfNot(nameof(StatusId), ProjectStatus.Draft)]
        [Range(1, int.MaxValue)]
        public int? ProjectMWCurrentlyAvailable { get; set; }
    }
}

using SE.Neo.Common.Attributes;
using SE.Neo.Common.Models.Shared;

namespace SE.Neo.Common.Models.Project
{
    public class ProjectOffsitePowerPurchaseAgreementDetailsDTO : BaseProjectDetailsDTO
    {
        [PropertyComparation("Location")]
        public double? Latitude { get; set; }

        [PropertyComparation("Location")]
        public double? Longitude { get; set; }

        [PropertyComparation("ISO / RTO")]
        public int? IsoRtoId { get; set; }

        public string? IsoRtoName { get; set; }

        [PropertyComparation("Product Type")]
        public int? ProductTypeId { get; set; }

        public string? ProductTypeName { get; set; }

        [PropertyComparation("Commercial Operation Date")]
        public DateTime? CommercialOperationDate { get; set; }

        [PropertyComparation("Value to Offtaker")]
        public IEnumerable<BaseIdNameDTO> ValuesToOfftakers { get; set; }

        [PropertyComparation("PPA Term Length")]
        public int? PPATermYearsLength { get; set; }

        [PropertyComparation("Total Project Nameplate Capacity")]
        public int? TotalProjectNameplateMWACCapacity { get; set; }

        [PropertyComparation("Total Project Expected Annual Production - P50")]
        public int? TotalProjectExpectedAnnualMWhProductionP50 { get; set; }

        [PropertyComparation("Minimum Offtake Volume Required")]
        public float? MinimumOfftakeMWhVolumeRequired { get; set; }

        [PropertyComparation("Notes for Potential Offtakers")]
        public string NotesForPotentialOfftakers { get; set; }

        [PropertyComparation("Settlement Type")]
        public int? SettlementTypeId { get; set; }

        public string? SettlementTypeName { get; set; }

        [PropertyComparation("Settlement Hub / Load Zone")]
        public int? SettlementHubOrLoadZoneId { get; set; }

        public string? SettlementHubOrLoadZoneName { get; set; }

        [PropertyComparation("Currency for all Price Entries")]
        public int? ForAllPriceEntriesCurrencyId { get; set; }

        public string? ForAllPriceEntriesCurrencyName { get; set; }

        [PropertyComparation("Contract Price (MWh)")]
        public string? ContractPricePerMWh { get; set; }

        [PropertyComparation("Floating Market Swap (Index, Discount)")]
        public string FloatingMarketSwapIndexDiscount { get; set; }

        [PropertyComparation("Floating Market Swap (Floor)")]
        public string FloatingMarketSwapFloor { get; set; }

        [PropertyComparation("Floating Market Swap (Cap)")]
        public string FloatingMarketSwapCap { get; set; }

        [PropertyComparation("Pricing Structure")]
        public int? PricingStructureId { get; set; }

        public string? PricingStructureName { get; set; }

        [PropertyComparation("Upside Percentage To Developer")]
        public int? UpsidePercentageToDeveloper { get; set; }

        [PropertyComparation("Upside Percentage To Offtaker")]
        public int? UpsidePercentageToOfftaker { get; set; }

        [PropertyComparation("Discount Amount")]
        public string DiscountAmount { get; set; }

        [PropertyComparation("EAC Type")]
        public int? EACId { get; set; }

        public string? EACName { get; set; }

        public string EACCustom { get; set; }

        [PropertyComparation("EAC Value")]
        public string EACValue { get; set; }

        [PropertyComparation("Settlement Price Interval")]
        public int? SettlementPriceIntervalId { get; set; }

        public string? SettlementPriceIntervalName { get; set; }

        public string SettlementPriceIntervalCustom { get; set; }

        [PropertyComparation("Settlement Calculation Interval")]
        public int? SettlementCalculationIntervalId { get; set; }

        public string? SettlementCalculationIntervalName { get; set; }

        public string AdditionalNotesForSEOperationsTeam { get; set; }

        [PropertyComparation("Project MW Currently Available")]
        public int? ProjectMWCurrentlyAvailable { get; set; }
    }
}

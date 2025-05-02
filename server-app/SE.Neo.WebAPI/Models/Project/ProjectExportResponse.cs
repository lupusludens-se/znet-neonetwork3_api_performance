namespace SE.Neo.WebAPI.Models.Project
{
    public class ProjectExportResponse
    {
        public string Title { get; set; }

        public string Category { get; set; }

        public string CategorySlug { get; set; }

        public string Technologies { get; set; }

        public string Regions { get; set; }

        public string StatusName { get; set; }

        public string PublishedOn { get; set; }

        public string Company { get; set; }

        public string ChangedOn { get; set; }

        public int? MinimumTermLengthAvailable { get; set; }

        public string? TimeAndUrgencyConsiderations { get; set; }

        public string? AdditionalComments { get; set; }

        public string? AdditionalCommentsEAC { get; set; }

        public string PublishedBy { get; set; }

        public string SubTitle { get; set; }

        public string Opportunity { get; set; }

        public string Description { get; set; }

        public int? TotalAnnualMWhAvailable { get; set; }

        public string UtilityTerritory { get; set; }

        public string ProjectCurrentlyAvailable { get; set; }

        public string IsInvestmentGradeCreditOfOfftakerRequired { get; set; }

        public string ProjectAvailabilityApproximateDate { get; set; }

        public string ContractStructures { get; set; }

        public string ValuesProvided { get; set; }

        public float? MinimumPurchaseVolume { get; set; }

        public string StripLengths { get; set; }

        public string PurchaseOptions { get; set; }

        public string UtilityName { get; set; }

        public string ProgramWebsite { get; set; }

        public string? TermLength { get; set; }//Utility Green Tariff

        public int? MinimumChargingStationsRequired { get; set; }

        public string FloatingMarketSwapFloor { get; set; }

        public string FloatingMarketSwapCap { get; set; }

        public int? UpsidePercentageToDeveloper { get; set; }

        public int? UpsidePercentageToOfftaker { get; set; }

        public string EACValue { get; set; }

        public int? ProjectMWCurrentlyAvailable { get; set; }

        public string? PricingStructureName { get; set; }
        public string FloatingMarketSwapIndexDiscount { get; set; }

        public string ContractPricePerMWh { get; set; }

        public string? ForAllPriceEntriesCurrencyName { get; set; }

        public string? SettlementTypeName { get; set; }

        public string? SettlementHubOrLoadZoneName { get; set; }

        public string DiscountAmount { get; set; }

        public string EACName { get; set; }

        public string SettlementPriceIntervalName { get; set; }

        public string AdditionalNotesForSEOperationsTeam { get; set; }

        public string? SettlementCalculationIntervalName { get; set; }

        public string SettlementPriceIntervalCustom { get; set; }

        public string? IsoRtoName { get; set; }

        public string? ProductTypeName { get; set; }

        public string NotesForPotentialOfftakers { get; set; }

        public string ValuesToOfftakers { get; set; }

        public float? MinimumOfftakeMWhVolumeRequired { get; set; }

        public int? TotalProjectExpectedAnnualMWhProductionP50 { get; set; }

        public int? TotalProjectNameplateMWACCapacity { get; set; }

        public int? PPATermYearsLength { get; set; }

        public string? CommercialOperationDate { get; set; }

        public string MinimumVolumeLoadRequired { get; set; }

        public float? MinimumAnnualSiteOnSiteSolar { get; set; }

        public float? MinimumAnnualSiteLoadFuelCells { get; set; }//units are different

        public float? MinimumAnnualSiteLoadRenewableRetailElectricity { get; set; }//units are different

        public float? MinimumAnnualPeakBatteryStorage { get; set; }

        public float? MinimumAnnualkWhPurchaseCommunitySolar { get; set; }

    }
}

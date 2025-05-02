namespace SE.Neo.WebAPI.Models.Project
{
    public class ProjectOffsitePowerPurchaseAgreementDetailsResponse : BaseProjectOffsitePowerPurchaseAgreementDetailsResponse
    {
        public int? SettlementTypeId { get; set; }
        public string? SettlementTypeName { get; set; }
        public int? SettlementHubOrLoadZoneId { get; set; }
        public string? SettlementHubOrLoadZoneName { get; set; }
        public int? ForAllPriceEntriesCurrencyId { get; set; }
        public string? ForAllPriceEntriesCurrencyName { get; set; }
        public string ContractPricePerMWh { get; set; }
        public string FloatingMarketSwapIndexDiscount { get; set; }
        public string FloatingMarketSwapFloor { get; set; }
        public string FloatingMarketSwapCap { get; set; }
        public int? PricingStructureId { get; set; }
        public string? PricingStructureName { get; set; }
        public int? UpsidePercentageToDeveloper { get; set; }
        public int? UpsidePercentageToOfftaker { get; set; }
        public string DiscountAmount { get; set; }
        public int? EACId { get; set; }
        public string? EACName { get; set; }
        public string EACCustom { get; set; }
        public string EACValue { get; set; }
        public int? SettlementPriceIntervalId { get; set; }
        public string? SettlementPriceIntervalName { get; set; }
        public string SettlementPriceIntervalCustom { get; set; }
        public int? SettlementCalculationIntervalId { get; set; }
        public string? SettlementCalculationIntervalName { get; set; }
        public string AdditionalNotesForSEOperationsTeam { get; set; }
        public int? ProjectMWCurrentlyAvailable { get; set; }
    }
}

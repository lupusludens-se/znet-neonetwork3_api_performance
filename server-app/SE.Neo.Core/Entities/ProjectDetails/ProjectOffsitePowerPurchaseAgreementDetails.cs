using SE.Neo.Core.Entities.ProjectDataTypes;
using SE.Neo.Core.Enums;
using SE.Neo.Core.Models.Project;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities.ProjectDetails
{
    [Table("Project_Offsite_Power_Purchase_Agreement_Details")]
    public class ProjectOffsitePowerPurchaseAgreementDetails : BaseProjectDetails
    {
        [Column("Project_Offsite_Power_Purchase_Agreement_Details")]
        public override int Id { get; set; }

        [Column("Latitude")]
        public double? Latitude { get; set; }

        [Column("Longitude")]
        public double? Longitude { get; set; }

        [Column("ISORTO_Id")]
        [ForeignKey("IsoRto")]
        public IsoRtoType? IsoRtoId { get; set; }

        public IsoRto? IsoRto { get; set; }

        [Column("Product_Type_Id")]
        [ForeignKey("ProductType")]
        public Enums.ProductType? ProductTypeId { get; set; }

        public ProjectDataTypes.ProductType? ProductType { get; set; }

        [Column("Commercial_Operation_Date")]
        public DateTime? CommercialOperationDate { get; set; }

        public ICollection<ProjectOffsitePPADetailsValueProvided> ValuesToOfftakers { get; set; }

        [Column("PPA_Term_Years_Length")]
        public int? PPATermYearsLength { get; set; }

        [Column("Total_Project_Nameplate_MWAC_Capacity")]
        public int? TotalProjectNameplateMWACCapacity { get; set; }

        [Column("Total_Project_Expected_Annual_MWh_Production_P50")]
        public int? TotalProjectExpectedAnnualMWhProductionP50 { get; set; }

        [Column("Minimum_Offtake_MWh_Volume_Required")]
        public float? MinimumOfftakeMWhVolumeRequired { get; set; }

        [Column("Notes_For_Potential_Offtakers")]
        [MaxLength(500)]
        public string NotesForPotentialOfftakers { get; set; }

        [Column("Settlement_Type_Id")]
        [ForeignKey("SettlementType")]
        public Enums.SettlementType? SettlementTypeId { get; set; }

        public ProjectDataTypes.SettlementType? SettlementType { get; set; }

        [Column("Settlement_Hub_Or_LoadZone_Id")]
        [ForeignKey("SettlementHubOrLoadZone")]
        public SettlementHubOrLoadZoneType? SettlementHubOrLoadZoneId { get; set; }

        public SettlementHubOrLoadZone? SettlementHubOrLoadZone { get; set; }

        [Column("For_All_Price_Entries_Currency_Id")]
        [ForeignKey("ForAllPriceEntriesCurrency")]
        public Enums.Currency? ForAllPriceEntriesCurrencyId { get; set; }

        public Currency? ForAllPriceEntriesCurrency { get; set; }

        [Column("Contract_Price_Per_MWh")]
        [MaxLength(100)]
        public string ContractPricePerMWh { get; set; }

        [Column("Floating_Market_Swap_Index_Discount")]
        [MaxLength(100)]
        public string FloatingMarketSwapIndexDiscount { get; set; }

        [Column("Floating_Market_Swap_Floor")]
        [MaxLength(100)]
        public string FloatingMarketSwapFloor { get; set; }

        [Column("Floating_Market_Swap_Cap")]
        [MaxLength(100)]
        public string FloatingMarketSwapCap { get; set; }

        [Column("Pricing_Structure_Id")]
        [ForeignKey("PricingStructure")]
        public PricingStructureType? PricingStructureId { get; set; }

        public PricingStructure? PricingStructure { get; set; }

        [Column("Upside_Percentage_To_Developer")]
        public int? UpsidePercentageToDeveloper { get; set; }

        [Column("Upside_Percentage_To_Offtaker")]
        public int? UpsidePercentageToOfftaker { get; set; }

        [Column("Discount_Amount")]
        [MaxLength(100)]
        public string DiscountAmount { get; set; }

        [Column("EAC_Id")]
        [ForeignKey("EAC")]
        public EACType? EACId { get; set; }

        public EAC? EAC { get; set; }

        [Column("EAC_Custom")]
        [MaxLength(100)]
        public string EACCustom { get; set; }

        [Column("EAC_Value")]
        [MaxLength(100)]
        public string EACValue { get; set; }

        [Column("Settlement_Price_Interval_Id")]
        [ForeignKey("SettlementPriceInterval")]
        public SettlementPriceIntervalType? SettlementPriceIntervalId { get; set; }

        public SettlementPriceInterval? SettlementPriceInterval { get; set; }

        [Column("Settlement_Price_Interval_Custom")]
        [MaxLength(100)]
        public string SettlementPriceIntervalCustom { get; set; }

        [Column("Settlement_Calculation_Interval_Id")]
        [ForeignKey("SettlementCalculationInterval")]
        public SettlementCalculationIntervalType? SettlementCalculationIntervalId { get; set; }

        public SettlementCalculationInterval? SettlementCalculationInterval { get; set; }

        [Column("Additional_Notes_For_SE_Operations_Team")]
        [MaxLength(500)]
        public string AdditionalNotesForSEOperationsTeam { get; set; }

        [Column("Project_MW_Currently_Available")]
        public int? ProjectMWCurrentlyAvailable { get; set; }
    }
}
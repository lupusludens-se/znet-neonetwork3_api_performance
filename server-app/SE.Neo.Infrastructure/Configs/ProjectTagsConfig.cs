using System.ComponentModel.DataAnnotations;

namespace SE.Neo.Infrastructure.Configs
{
    public class ProjectTagsConfig
    {
        [Required]
        public string OffsitePPATags { get; set; }

        [Required]
        public string OnsiteSolarTags { get; set; }

        [Required]
        public string BatteryStorageTags { get; set; }

        [Required]
        public string FuelCellsTags { get; set; }

        [Required]
        public string CommunitySolarTags { get; set; }

        [Required]
        public string CarbonOffsetsTags { get; set; }

        [Required]
        public string EACTags { get; set; }

        [Required]
        public string EfficiencyAuditsAndConsultingTags { get; set; }

        [Required]
        public string EfficiencyEquipmentMeasuresTags { get; set; }

        [Required]
        public string EmergingTechnologyTags { get; set; }

        [Required]
        public string EVChargingTags { get; set; }

        [Required]
        public string GreenTariffsTags { get; set; }

        [Required]
        public string RenewableRetailTags { get; set; }

        [Required]
        public string AggregatedPowerPurchaseAgreementTags { get; set; }

        [Required]
        public string MarketBriefTags { get; set; }

    }
}

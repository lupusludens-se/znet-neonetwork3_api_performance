using System.ComponentModel;

namespace SE.Neo.Core.Enums
{
    public enum ValueProvidedType
    {
        [Description("Cost Savings")]
        CostSavings = 1,

        [Description("Environmental Attributes and/or Carbon Reduction Targets")]
        EnvironmentalCarbonReductionTargets,

        [Description("Story/Publicity")]
        StoryPublicity,

        [Description("Resiliency")]
        Resiliency,

        [Description("Other")]
        Other,

        [Description("Renewable Attributes")]
        RenewableAttributes,

        [Description("Energy Arbitrage")]
        EnergyArbitrage,

        [Description("Visible Commitment to Climate Action")]
        MitigatingClimateChange,

        [Description("Community Benefits")]
        CommunityBenefits,

        [Description("Energy Savings/Reduction")]
        EnergySavings,

        [Description("Greenhouse Gas Emission Reduction Offset")]
        GreenhouseGasEmission
    }
}

using SE.Neo.Core.Enums;

namespace SE.Neo.WebAPI.Enums
{
    /// <summary>
    /// Represents the types of value provided in a quote.
    /// </summary>
    public enum QuoteValueProvidedType : int
    {
        /// <summary>
        /// Represents cost savings.
        /// </summary>
        CostSavings = ValueProvidedType.CostSavings,

        /// <summary>
        /// Represents environmental carbon reduction targets.
        /// </summary>
        EnvironmentalCarbonReductionTargets = ValueProvidedType.EnvironmentalCarbonReductionTargets,

        /// <summary>
        /// Represents story publicity.
        /// </summary>
        StoryPublicity = ValueProvidedType.StoryPublicity,

        /// <summary>
        /// Represents resiliency.
        /// </summary>
        Resiliency = ValueProvidedType.Resiliency,

        /// <summary>
        /// Represents other types of value provided.
        /// </summary>
        Other = ValueProvidedType.Other
    }
}

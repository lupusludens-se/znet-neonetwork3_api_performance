using SE.Neo.Core.Enums;

namespace SE.Neo.WebAPI.Enums
{
    /// <summary>
    /// Specifies the types of values provided by EV charging projects.
    /// </summary>
    public enum ProjectEVChargingValueProvidedType
    {
        /// <summary>
        /// Represents cost savings provided by the EV charging project.
        /// </summary>
        CostSavings = ValueProvidedType.CostSavings,

        /// <summary>
        /// Represents the mitigation of climate change provided by the EV charging project.
        /// </summary>
        MitigatingClimateChange = ValueProvidedType.MitigatingClimateChange,

        /// <summary>
        /// Represents other types of values provided by the EV charging project.
        /// </summary>
        Other = ValueProvidedType.Other
    }
}

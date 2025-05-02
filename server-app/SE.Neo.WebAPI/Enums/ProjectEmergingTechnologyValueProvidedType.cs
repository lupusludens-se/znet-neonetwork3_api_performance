using SE.Neo.Core.Enums;

namespace SE.Neo.WebAPI.Enums
{
    /// <summary>
    /// Enum representing the types of value provided by emerging technology projects.
    /// </summary>
    public enum ProjectEmergingTechnologyValueProvidedType
    {
        /// <summary>
        /// Represents cost savings provided by the project.
        /// </summary>
        CostSavings = ValueProvidedType.CostSavings,

        /// <summary>
        /// Represents resiliency provided by the project.
        /// </summary>
        Resiliency = ValueProvidedType.Resiliency,

        /// <summary>
        /// Represents mitigation of climate change provided by the project.
        /// </summary>
        MitigatingClimateChange = ValueProvidedType.MitigatingClimateChange,

        /// <summary>
        /// Represents other types of value provided by the project.
        /// </summary>
        Other = ValueProvidedType.Other
    }
}

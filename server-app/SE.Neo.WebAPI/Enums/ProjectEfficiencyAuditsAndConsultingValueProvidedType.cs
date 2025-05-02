using SE.Neo.Core.Enums;

namespace SE.Neo.WebAPI.Enums
{
    /// <summary>
    /// Specifies the types of value provided by project efficiency audits and consulting.
    /// </summary>
    public enum ProjectEfficiencyAuditsAndConsultingValueProvidedType
    {
        /// <summary>
        /// Represents cost savings provided by the project.
        /// </summary>
        CostSavings = ValueProvidedType.CostSavings,

        /// <summary>
        /// Represents energy savings provided by the project.
        /// </summary>
        EnergySavings = ValueProvidedType.EnergySavings,

        /// <summary>
        /// Represents resiliency improvements provided by the project.
        /// </summary>
        Resiliency = ValueProvidedType.Resiliency,

        /// <summary>
        /// Represents other types of value provided by the project.
        /// </summary>
        Other = ValueProvidedType.Other
    }
}

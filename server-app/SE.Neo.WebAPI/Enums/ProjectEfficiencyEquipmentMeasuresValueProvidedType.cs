using SE.Neo.Core.Enums;

namespace SE.Neo.WebAPI.Enums
{
    /// <summary>
    /// Specifies the types of value provided by project efficiency equipment measures.
    /// </summary>
    public enum ProjectEfficiencyEquipmentMeasuresValueProvidedType
    {
        /// <summary>
        /// Represents cost savings provided by the equipment measures.
        /// </summary>
        CostSavings = ValueProvidedType.CostSavings,

        /// <summary>
        /// Represents energy savings provided by the equipment measures.
        /// </summary>
        EnergySavings = ValueProvidedType.EnergySavings,

        /// <summary>
        /// Represents resiliency provided by the equipment measures.
        /// </summary>
        Resiliency = ValueProvidedType.Resiliency,

        /// <summary>
        /// Represents other types of value provided by the equipment measures.
        /// </summary>
        Other = ValueProvidedType.Other
    }
}

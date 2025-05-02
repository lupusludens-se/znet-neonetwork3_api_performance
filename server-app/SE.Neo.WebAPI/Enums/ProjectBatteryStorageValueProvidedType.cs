using SE.Neo.Core.Enums;

namespace SE.Neo.WebAPI.Enums
{
    /// <summary>
    /// Specifies the types of values provided by a project battery storage.
    /// </summary>
    public enum ProjectBatteryStorageValueProvidedType
    {
        /// <summary>
        /// Represents cost savings provided by the battery storage.
        /// </summary>
        CostSavings = ValueProvidedType.CostSavings,

        /// <summary>
        /// Represents resiliency provided by the battery storage.
        /// </summary>
        Resiliency = ValueProvidedType.Resiliency,

        /// <summary>
        /// Represents other types of values provided by the battery storage.
        /// </summary>
        Other = ValueProvidedType.Other
    }
}

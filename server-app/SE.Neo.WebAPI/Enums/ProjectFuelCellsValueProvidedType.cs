using SE.Neo.Core.Enums;

namespace SE.Neo.WebAPI.Enums
{
    /// <summary>
    /// Specifies the types of value provided by project fuel cells.
    /// </summary>
    public enum ProjectFuelCellsValueProvidedType
    {
        /// <summary>
        /// Represents cost savings provided by the project fuel cells.
        /// </summary>
        CostSavings = ValueProvidedType.CostSavings,

        /// <summary>
        /// Represents renewable attributes provided by the project fuel cells.
        /// </summary>
        RenewableAttributes = ValueProvidedType.RenewableAttributes,

        /// <summary>
        /// Represents resiliency provided by the project fuel cells.
        /// </summary>
        Resiliency = ValueProvidedType.Resiliency,

        /// <summary>
        /// Represents other types of value provided by the project fuel cells.
        /// </summary>
        Other = ValueProvidedType.Other
    }
}

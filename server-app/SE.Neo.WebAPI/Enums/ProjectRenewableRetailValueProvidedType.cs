using SE.Neo.Core.Enums;

namespace SE.Neo.WebAPI.Enums
{
    /// <summary>
    /// Specifies the types of value provided by renewable retail projects.
    /// </summary>
    public enum ProjectRenewableRetailValueProvidedType
    {
        /// <summary>
        /// Represents cost savings provided by the project.
        /// </summary>
        CostSavings = ValueProvidedType.CostSavings,

        /// <summary>
        /// Represents renewable attributes provided by the project.
        /// </summary>
        RenewableAttributes = ValueProvidedType.RenewableAttributes,

        /// <summary>
        /// Represents other types of value provided by the project.
        /// </summary>
        Other = ValueProvidedType.Other
    }
}

using SE.Neo.Core.Enums;

namespace SE.Neo.WebAPI.Enums
{
    /// <summary>
    /// Specifies the types of values provided by project green tariffs.
    /// </summary>
    public enum ProjectGreenTariffsValueProvidedType
    {
        /// <summary>
        /// Represents cost savings provided by the project green tariffs.
        /// </summary>
        CostSavings = ValueProvidedType.CostSavings,

        /// <summary>
        /// Represents renewable attributes provided by the project green tariffs.
        /// </summary>
        RenewableAttributes = ValueProvidedType.RenewableAttributes,

        /// <summary>
        /// Represents other types of values provided by the project green tariffs.
        /// </summary>
        Other = ValueProvidedType.Other
    }
}

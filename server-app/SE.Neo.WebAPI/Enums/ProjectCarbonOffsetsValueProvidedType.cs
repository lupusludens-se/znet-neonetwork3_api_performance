using SE.Neo.Core.Enums;

namespace SE.Neo.WebAPI.Enums
{
    /// <summary>
    /// Specifies the types of values provided for project carbon offsets.
    /// </summary>
    public enum ProjectCarbonOffsetsValueProvidedType
    {
        /// <summary>
        /// Represents the greenhouse gas emission value provided type.
        /// </summary>
        GreenhouseGasEmission = ValueProvidedType.GreenhouseGasEmission,

        /// <summary>
        /// Represents other types of value provided.
        /// </summary>
        Other = ValueProvidedType.Other
    }
}

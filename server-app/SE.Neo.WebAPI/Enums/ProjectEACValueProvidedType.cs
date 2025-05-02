using SE.Neo.Core.Enums;

namespace SE.Neo.WebAPI.Enums
{
    /// <summary>
    /// Specifies the type of value provided for Project EAC.
    /// </summary>
    public enum ProjectEACValueProvidedType
    {
        /// <summary>
        /// Represents renewable attributes.
        /// </summary>
        RenewableAttributes = ValueProvidedType.RenewableAttributes,

        /// <summary>
        /// Represents other types of values.
        /// </summary>
        Other = ValueProvidedType.Other
    }
}

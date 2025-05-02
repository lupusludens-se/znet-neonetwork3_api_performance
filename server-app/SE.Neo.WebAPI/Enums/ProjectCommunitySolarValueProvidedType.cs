using SE.Neo.Core.Enums;

namespace SE.Neo.WebAPI.Enums
{
    /// <summary>
    /// Specifies the types of value provided by community solar projects.
    /// </summary>
    public enum ProjectCommunitySolarValueProvidedType
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
        /// Represents the mitigation of climate change provided by the project.
        /// </summary>
        MitigatingClimateChange = ValueProvidedType.MitigatingClimateChange,

        /// <summary>
        /// Represents community benefits provided by the project.
        /// </summary>
        CommunityBenefits = ValueProvidedType.CommunityBenefits,

        /// <summary>
        /// Represents other types of value provided by the project.
        /// </summary>
        Other = ValueProvidedType.Other
    }
}

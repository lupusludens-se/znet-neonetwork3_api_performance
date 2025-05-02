using SE.Neo.Core.Enums;

namespace SE.Neo.WebAPI.Enums
{
    /// <summary>
    /// Specifies the types of values provided by a project offsite PPA.
    /// </summary>
    public enum ProjectOffsitePPAValueProvidedType
    {
        /// <summary>
        /// Represents renewable attributes provided by the project.
        /// </summary>
        RenewableAttributes = ValueProvidedType.RenewableAttributes,

        /// <summary>
        /// Represents energy arbitrage provided by the project.
        /// </summary>
        EnergyArbitrage = ValueProvidedType.EnergyArbitrage,

        /// <summary>
        /// Represents mitigating climate change provided by the project.
        /// </summary>
        MitigatingClimateChange = ValueProvidedType.MitigatingClimateChange,

        /// <summary>
        /// Represents community benefits provided by the project.
        /// </summary>
        CommunityBenefits = ValueProvidedType.CommunityBenefits,
    }
}

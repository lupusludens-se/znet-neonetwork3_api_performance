using SE.Neo.Core.Enums;

namespace SE.Neo.WebAPI.Enums
{
    /// <summary>
    /// Represents the types of contract structures for community solar projects.
    /// </summary>
    public enum ProjectCommunitySolarContractStructureType
    {
        /// <summary>
        /// Contract structure type where the discount is applied to the tariff.
        /// </summary>
        DiscountToTariff = ContractStructureType.DiscountToTariff,

        /// <summary>
        /// Other types of contract structures.
        /// </summary>
        Other = ContractStructureType.Other
    }
}

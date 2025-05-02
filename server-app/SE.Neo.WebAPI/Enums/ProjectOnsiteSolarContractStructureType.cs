using SE.Neo.Core.Enums;

namespace SE.Neo.WebAPI.Enums
{
    /// <summary>
    /// Represents the different types of contract structures for onsite solar projects.
    /// </summary>
    public enum ProjectOnsiteSolarContractStructureType
    {
        /// <summary>
        /// A contract structure where the customer agrees to purchase power at a set rate.
        /// </summary>
        PowerPurchaseAgreement = ContractStructureType.PowerPurchaseAgreement,

        /// <summary>
        /// A contract structure where the customer leases the solar equipment.
        /// </summary>
        Lease = ContractStructureType.Lease,

        /// <summary>
        /// A contract structure where the customer purchases the solar equipment outright.
        /// </summary>
        CashPurchase = ContractStructureType.CashPurchase,

        /// <summary>
        /// A contract structure where savings are shared between the provider and the customer.
        /// </summary>
        SharedSavings = ContractStructureType.SharedSavings,

        /// <summary>
        /// A contract structure where savings are guaranteed by the provider.
        /// </summary>
        GuaranteedSavings = ContractStructureType.GuaranteedSavings
    }
}

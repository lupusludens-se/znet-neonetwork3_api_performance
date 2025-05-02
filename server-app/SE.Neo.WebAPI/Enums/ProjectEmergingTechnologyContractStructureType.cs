using SE.Neo.Core.Enums;

namespace SE.Neo.WebAPI.Enums
{
    /// <summary>
    /// Represents the types of contract structures for emerging technology projects.
    /// </summary>
    public enum ProjectEmergingTechnologyContractStructureType
    {
        /// <summary>
        /// A contract structure where power is purchased.
        /// </summary>
        PowerPurchaseAgreement = ContractStructureType.PowerPurchaseAgreement,

        /// <summary>
        /// A contract structure based on leasing.
        /// </summary>
        Lease = ContractStructureType.Lease,

        /// <summary>
        /// A contract structure based on cash purchase.
        /// </summary>
        CashPurchase = ContractStructureType.CashPurchase,

        /// <summary>
        /// A contract structure based on shared savings.
        /// </summary>
        SharedSavings = ContractStructureType.SharedSavings,

        /// <summary>
        /// A contract structure based on guaranteed savings.
        /// </summary>
        GuaranteedSavings = ContractStructureType.GuaranteedSavings
    }
}

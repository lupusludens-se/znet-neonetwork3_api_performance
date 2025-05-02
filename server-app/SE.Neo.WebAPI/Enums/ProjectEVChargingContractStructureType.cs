using SE.Neo.Core.Enums;

namespace SE.Neo.WebAPI.Enums
{
    /// <summary>
    /// Specifies the different types of contract structures available for EV charging projects.
    /// </summary>
    public enum ProjectEVChargingContractStructureType
    {
        /// <summary>
        /// A contract structure where the power is purchased through an agreement.
        /// </summary>
        PowerPurchaseAgreement = ContractStructureType.PowerPurchaseAgreement,

        /// <summary>
        /// A contract structure based on leasing.
        /// </summary>
        Lease = ContractStructureType.Lease,

        /// <summary>
        /// A contract structure where the purchase is made with cash.
        /// </summary>
        CashPurchase = ContractStructureType.CashPurchase,

        /// <summary>
        /// A contract structure where savings are shared between parties.
        /// </summary>
        SharedSavings = ContractStructureType.SharedSavings,

        /// <summary>
        /// A contract structure where savings are guaranteed.
        /// </summary>
        GuaranteedSavings = ContractStructureType.GuaranteedSavings
    }
}

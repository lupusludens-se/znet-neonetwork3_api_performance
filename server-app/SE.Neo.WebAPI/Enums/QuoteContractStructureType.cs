using SE.Neo.Core.Enums;

namespace SE.Neo.WebAPI.Enums
{
    /// <summary>
    /// Represents the types of contract structures for quotes.
    /// </summary>
    public enum QuoteContractStructureType : int
    {
        /// <summary>
        /// Represents a cash purchase contract structure.
        /// </summary>
        CashPurchase = ContractStructureType.CashPurchase,

        /// <summary>
        /// Represents a power purchase agreement contract structure.
        /// </summary>
        PowerPurchaseAgreement = ContractStructureType.PowerPurchaseAgreement,

        /// <summary>
        /// Represents other types of contract structures.
        /// </summary>
        Other = ContractStructureType.Other
    }
}

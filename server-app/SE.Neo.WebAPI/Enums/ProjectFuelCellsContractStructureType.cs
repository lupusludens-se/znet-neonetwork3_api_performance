using SE.Neo.Core.Enums;

namespace SE.Neo.WebAPI.Enums
{
    /// <summary>
    /// Represents the different types of contract structures for project fuel cells.
    /// </summary>
    public enum ProjectFuelCellsContractStructureType
    {
        /// <summary>
        /// Power Purchase Agreement contract structure.
        /// </summary>
        PowerPurchaseAgreement = ContractStructureType.PowerPurchaseAgreement,

        /// <summary>
        /// Lease contract structure.
        /// </summary>
        Lease = ContractStructureType.Lease,

        /// <summary>
        /// Cash Purchase contract structure.
        /// </summary>
        CashPurchase = ContractStructureType.CashPurchase,

        /// <summary>
        /// Shared Savings contract structure.
        /// </summary>
        SharedSavings = ContractStructureType.SharedSavings,

        /// <summary>
        /// Guaranteed Savings contract structure.
        /// </summary>
        GuaranteedSavings = ContractStructureType.GuaranteedSavings,

        /// <summary>
        /// Other types of contract structures.
        /// </summary>
        Other = ContractStructureType.Other
    }
}

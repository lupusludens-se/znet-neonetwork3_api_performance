using SE.Neo.Core.Enums;

namespace SE.Neo.WebAPI.Enums
{
    /// <summary>
    /// Specifies the contract structure types for project efficiency audits and consulting.
    /// </summary>
    public enum ProjectEfficiencyAuditsAndConsultingContractStructureType
    {
        /// <summary>
        /// Represents a cash purchase contract structure.
        /// </summary>
        CashPurchase = ContractStructureType.CashPurchase,

        /// <summary>
        /// Represents a contract structure as a service or alternative financing.
        /// </summary>
        AsAServiceOrAlternativeFinancing = ContractStructureType.AsAServiceOrAlternativeFinancing,

        /// <summary>
        /// Represents a shared savings contract structure.
        /// </summary>
        SharedSavings = ContractStructureType.SharedSavings,

        /// <summary>
        /// Represents a guaranteed savings contract structure.
        /// </summary>
        GuaranteedSavings = ContractStructureType.GuaranteedSavings
    }
}

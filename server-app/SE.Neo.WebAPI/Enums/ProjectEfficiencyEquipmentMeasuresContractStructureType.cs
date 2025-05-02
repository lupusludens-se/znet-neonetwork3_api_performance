using SE.Neo.Core.Enums;

namespace SE.Neo.WebAPI.Enums
{
    /// <summary>
    /// Represents the types of contract structures for project efficiency equipment measures.
    /// </summary>
    public enum ProjectEfficiencyEquipmentMeasuresContractStructureType
    {
        /// <summary>
        /// Represents a cash purchase contract structure.
        /// </summary>
        CashPurchase = ContractStructureType.CashPurchase,

        /// <summary>
        /// Represents a contract structure that is either as-a-service or alternative financing.
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

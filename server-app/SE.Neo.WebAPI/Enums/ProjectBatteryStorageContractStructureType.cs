
using SE.Neo.Core.Enums;

namespace SE.Neo.WebAPI.Enums
{
    /// <summary>  
    /// Enum representing different types of project battery storage contract structures.  
    /// </summary>  
    public enum ProjectBatteryStorageContractStructureType
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
        GuaranteedSavings = ContractStructureType.GuaranteedSavings
    }
}

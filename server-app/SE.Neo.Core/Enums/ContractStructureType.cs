using System.ComponentModel;

namespace SE.Neo.Core.Enums
{
    /// <summary>
    /// 
    /// </summary>
    public enum ContractStructureType : int
    {
        [Description("Cash Purchase")]
        CashPurchase = 1,

        [Description("Power Purchase Agreement")]
        PowerPurchaseAgreement,

        [Description("Other")]
        Other,

        [Description("Lease")]
        Lease,

        [Description("Shared Savings")]
        SharedSavings,

        [Description("Guaranteed Savings")]
        GuaranteedSavings,

        [Description("Discount to Tariff")]
        DiscountToTariff,

        [Description("As-a-Service or Alternative Financing")]
        AsAServiceOrAlternativeFinancing
    }
}

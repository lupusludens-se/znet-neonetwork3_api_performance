using System.ComponentModel;

namespace SE.Neo.Core.Enums
{
    public enum ProductType
    {
        [Description("Energy Only")]
        EnergyOnly = 1,

        [Description("Energy with Project EACs")]
        EnergyWithProjectEACs,

        [Description("Energy with Certified Swap EACs")]
        EnergyWithCertifiedSwapEACs,

        [Description("Retail Delivered Product")]
        RetailDelivered
    }
}

using System.ComponentModel;

namespace SE.Neo.Core.Enums
{
    public enum PricingStructureType
    {
        [Description("Plain CFD")]
        PlainCFD = 1,

        [Description("Upside Share")]
        UpsideShare,

        [Description("Market Following")]
        MarketFollowing,

        [Description("Fixed Discount to Market")]
        FixedDiscountToMarket
    }
}

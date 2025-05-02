using System.ComponentModel;

namespace SE.Neo.Core.Enums
{
    public enum PurchaseOptionType
    {
        [Description("Behind the Meter")]
        BehindTheMeter = 1,

        [Description("In front of the Meter")]
        InFrontOfTheMeter
    }
}

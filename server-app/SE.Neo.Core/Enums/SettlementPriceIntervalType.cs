using System.ComponentModel;

namespace SE.Neo.Core.Enums
{
    public enum SettlementPriceIntervalType
    {
        [Description("Day-Ahead")]
        DayAhead = 1,

        [Description("Real-Time")]
        RealTime,

        [Description("Intraday")]
        Intraday,

        [Description("Other")]
        Other
    }
}

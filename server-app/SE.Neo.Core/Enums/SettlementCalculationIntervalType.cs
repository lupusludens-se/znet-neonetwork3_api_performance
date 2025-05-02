using System.ComponentModel;

namespace SE.Neo.Core.Enums
{
    public enum SettlementCalculationIntervalType
    {
        [Description("Hourly")]
        Hourly = 1,

        [Description("Monthly")]
        Monthly,

        [Description("Semi-annual")]
        SemiAnnual,

        [Description("Annual")]
        Annual
    }
}

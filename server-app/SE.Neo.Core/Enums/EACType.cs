using System.ComponentModel;

namespace SE.Neo.Core.Enums
{
    public enum EACType
    {
        [Description("REC")]
        REC = 1,
        [Description("Green-E REC")]
        GreenERec,
        [Description("GO")]
        Go,
        [Description("REGO")]
        Rego,
        [Description("I-REC")]
        IRec,
        [Description("LGC")]
        Lgc,
        [Description("Other")]
        Other
    }
}


using System.ComponentModel;

namespace SE.Neo.Core.Enums
{
    public enum TermLengthType
    {
        [Description("12 months")]
        Month12 = 1,

        [Description("24 months")]
        Month24,

        [Description("36 months")]
        Month36,

        [Description("> 36 months")]
        MoreThan36Month
    }
}

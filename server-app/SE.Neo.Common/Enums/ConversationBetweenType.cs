using System.ComponentModel;

namespace SE.Neo.Common.Enums
{
    public enum ConversationBetweenType
    {
        [Description("NA")]
        NA = -1,

        [Description("Corporation - Corporation")]
        CorpToCorp = 0,

        [Description("Corporation - Solution Provider")]
        CorpToSp = 1,

        [Description("Solution Provider - Corporation")]
        SpToCorp = 2,

        [Description("Solution Provider - Solution Provider")]
        SpToSp = 3
    }

}

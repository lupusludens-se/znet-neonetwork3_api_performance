using System.ComponentModel;

namespace SE.Neo.Core.Enums
{
    public enum ArticleType
    {
        [Description("Video")]
        Video = 13,

        [Description("Audio")]
        Audio = 14,

        [Description("PDF")]
        PDF = 15,

        [Description("Market Brief")]
        MarketBrief = 16,

        [Description("Articles")]
        Articles = 17
    }
}
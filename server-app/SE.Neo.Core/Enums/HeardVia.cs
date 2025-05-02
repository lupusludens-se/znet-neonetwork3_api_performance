using System.ComponentModel;

namespace SE.Neo.Core.Enums
{
    public enum HeardVia : int
    {
        [Description("Conference/Event")]
        ConferenceEvent = 1,

        [Description("Co-Worker")]
        CoWorker,

        [Description("Zeigo Network Member")]
        NeoMember,

        [Description("News/Article")]
        NewsArticle,

        [Description("Referred by Schneider Electric Contact")]
        SEContact,

        [Description("Social Media")]
        SocialMedia,

        [Description("Web Search")]
        WebSearch,

        [Description("I am an Employee")]
        Employee,

        [Description("Other")]
        Other
    }
}
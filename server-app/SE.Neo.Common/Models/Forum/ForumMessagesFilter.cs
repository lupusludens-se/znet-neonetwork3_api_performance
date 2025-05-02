using SE.Neo.Common.Models.Shared;

namespace SE.Neo.Common.Models.Forum
{
    public class ForumMessagesFilter : BaseSearchFilterModel
    {
        /// <summary>
        /// Unique identifier of the message to get it replies.
        /// </summary>
        public int? ParentMessageId { get; set; }
    }
}

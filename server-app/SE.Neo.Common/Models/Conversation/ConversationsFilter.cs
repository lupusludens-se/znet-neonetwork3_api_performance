using SE.Neo.Common.Enums;
using SE.Neo.Common.Models.Shared;

namespace SE.Neo.Common.Models.Conversation
{
    public class ConversationsFilter : BaseSearchFilterModel
    {
        public bool IncludeAll { get; set; }
        public int? WithUserId { get; set; }
        public bool? Individual { get; set; }

        public List<ConversationBetweenType>? ConversationType { get; set; } = new List<ConversationBetweenType>();
    }
}

using SE.Neo.Core.Entities;

namespace SE.Neo.Core.Models.Conversation
{
    public class MessageGroupedByConversation
    {
        public ConversationKey Key { get; set; }

        public Message Message { get; set; }

        public class ConversationKey
        {
            public int ConversationId { get; set; }
        }
    }
}

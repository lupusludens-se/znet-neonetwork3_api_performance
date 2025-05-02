using SE.Neo.WebAPI.Models.Project;

namespace SE.Neo.WebAPI.Models.Conversation
{
    public class ConversationResponse
    {
        /// <summary>
        /// Unique identifier of the conversation.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The topic of conversation.
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Number of unread messages in the given conversation.
        /// </summary>
        public int UnreadCount { get; set; }

        public int SourceTypeId { get; set; }

        public string SourceTypeName { get; set; }

        /// <summary>
        /// Project object.
        /// </summary>
        public ProjectResponse? Project { get; set; }

        /// <summary>
        /// Last message in the given conversation.
        /// </summary>
        public ConversationMessageResponse LastMessage { get; set; }

        /// <summary>
        /// List of the users in the conversation.
        /// </summary>
        public IEnumerable<ConversationUserResponse> Users { get; set; }
    }
}
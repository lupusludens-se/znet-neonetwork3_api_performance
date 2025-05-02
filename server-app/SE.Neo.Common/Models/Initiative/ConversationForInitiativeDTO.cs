using SE.Neo.Common.Models.Conversation;

namespace SE.Neo.Common.Models.Initiative
{
    public class ConversationForInitiativeDTO
    {
        public int Id { get; set; }

        public int CreatedByUserId { get; set; }

        public string Subject { get; set; }

        public int UnreadCount { get; set; }

        public int SourceTypeId { get; set; }

        //public int? ProjectId { get; set; }

        //public ProjectDTO? Project { get; set; }

        public ConversationMessageDTO LastMessage { get; set; }

        public IEnumerable<ConversationUserForInitiativeDTO> Users { get; set; }

        public int UsersCount { get; set; }
    }
}

using SE.Neo.Common.Models.Project;

namespace SE.Neo.Common.Models.Conversation
{
    public class ConversationDTO
    {
        public int Id { get; set; }

        public int CreatedByUserId { get; set; }

        public string Subject { get; set; }

        public int UnreadCount { get; set; }

        public int SourceTypeId { get; set; }

        public string SourceTypeName { get; set; }

        public int? ProjectId { get; set; }

        public ProjectDTO? Project { get; set; }

        public ConversationMessageDTO LastMessage { get; set; }

        public IEnumerable<ConversationUserDTO> Users { get; set; }
    }
}
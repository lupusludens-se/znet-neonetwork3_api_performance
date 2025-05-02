using SE.Neo.Common.Enums;
using SE.Neo.Common.Models.Shared;

namespace SE.Neo.Common.Models.Conversation
{
    public class ConversationMessageDTO
    {
        public int Id { get; set; }

        public int ConversationId { get; set; }

        public string? Text { get; set; }

        public DateTime? CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public int UserId { get; set; }

        public ConversationUserDTO User { get; set; }

        public IEnumerable<AttachmentDTO> Attachments { get; set; }
        public MessageStatus StatusId { get; set; }
    }
}
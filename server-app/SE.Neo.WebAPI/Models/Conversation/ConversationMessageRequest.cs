using SE.Neo.WebAPI.Models.Shared;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Models.Conversation
{
    public class ConversationMessageRequest
    {
        /// <summary>
        /// Text of the conversation message.
        /// </summary>
        [MaxLength(6000)]
        public string? Text { get; set; }

        /// <summary>
        /// List of the message attachments.
        /// </summary>
        public List<MessageAttachmentRequest>? Attachments { get; set; }
    }
}

using SE.Neo.Common.Enums;

namespace SE.Neo.WebAPI.Models.Conversation
{
    public class ConversationMessageResponse
    {
        /// <summary>
        /// Unique identifier of the message.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Text of the message.
        /// </summary>
        public string? Text { get; set; }

        /// <summary>
        /// Time of creation.
        /// </summary>
        public DateTime? CreatedOn { get; set; }

        /// <summary>
        /// Time of creation.
        /// </summary>
        public DateTime? ModifiedOn { get; set; }

        /// <summary>
        /// User object who sent message.
        /// </summary>
        public ConversationUserResponse User { get; set; }

        /// <summary>
        /// List of the message attachments.
        /// </summary>
        public IEnumerable<AttachmentResponse> Attachments { get; set; }


        public MessageStatus StatusId { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Models.Conversation
{
    public class ConversationContactUsRequest
    {
        /// <summary>
        /// The topic of conversation.
        /// </summary>
        [Required]
        [MaxLength(250)]
        public string Subject { get; set; }

        /// <summary>
        ///  Message of the conversation.
        /// </summary>
        [Required]
        [MaxLength(4000)]
        public string Message { get; set; }
    }
}
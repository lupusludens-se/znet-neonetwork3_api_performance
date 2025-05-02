using SE.Neo.Core.Enums;
using SE.Neo.WebAPI.Validation;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Models.Conversation
{
    public class ConversationRequest
    {
        /// <summary>
        /// The topic of conversation.
        /// </summary>
        [Required]
        [MaxLength(250)]
        public string Subject { get; set; }

        [EnumExist(typeof(DiscussionSourceType), "must contain discussion source type id")]
        public DiscussionSourceType SourceTypeId { get; set; }

        /// <summary>
        /// Unique identifier of the project.
        /// </summary>
        [ProjectIdExist]
        public int? ProjectId { get; set; }

        /// <summary>
        /// First message of the conversation.
        /// </summary>
        [Required]
        public ConversationMessageRequest Message { get; set; }

        /// <summary>
        /// List of the users in the conversation.
        /// </summary>
        [Required]
        public List<ConversationUserRequest> Users { get; set; }
    }
}

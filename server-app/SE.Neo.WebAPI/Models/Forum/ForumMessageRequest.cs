using SE.Neo.Common.Enums;
using SE.Neo.WebAPI.Models.Shared;
using SE.Neo.WebAPI.Validation;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Models.Forum
{
    public class ForumMessageRequest
    {
        /// <summary>
        /// Text of the forum message/reply.
        /// </summary>
        [Required]
        [MaxWordAttributes(10000, ErrorMessage = "There are too many characters in {0}.")]
        public string? Text { get; set; }

        /// <summary>
        /// Text of the forum message/reply without html content.
        /// </summary>
        [Required]
        [MaxWordAttributes(6000, ErrorMessage = "There are too many characters in {0}.")]
        public string? TextContent { get; set; }

        /// <summary>
        /// Unique identifier of the message. If exists, message will be reply to it.
        /// </summary>
        [ForumMessageIdExist]
        public int? ParentMessageId { get; set; }

        /// <summary>
        /// Defines is message pinned. This value defaults to false.
        /// </summary>
        public bool IsPinned { get; set; }

        /// <summary>
        /// List of the forum attachments. Only Link type allowed.
        /// </summary>
        [MessageAttachmentType(AttachmentType.Link)]
        public List<MessageAttachmentRequest>? Attachments { get; set; }

        /// <summary>
        /// Current User Id
        /// </summary>
        public int UserId { get; set; }
    }
}

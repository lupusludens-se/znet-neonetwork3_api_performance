using SE.Neo.WebAPI.Models.Shared;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Models.Forum
{
    public class ForumFirstMessageRequest
    {
        /// <summary>
        /// Description of the forum.
        /// </summary>
        [Required]
        [MaxWordAttributes(10000, ErrorMessage = "There are too many characters in {0}.")]
        public string? Text { get; set; }

        /// <summary>
        /// Description of the forum without html content.
        /// </summary>
        [Required]
        [MaxWordAttributes(6000, ErrorMessage = "There are too many characters in {0}.")]
        public string? TextContent { get; set; }

        /// <summary>
        /// Message Id (will be null for create forum)
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// List of the forum attachments.
        /// </summary>
        public List<MessageAttachmentRequest>? Attachments { get; set; }
    }
}

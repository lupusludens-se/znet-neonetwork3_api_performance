namespace SE.Neo.WebAPI.Models.Forum
{
    public class ForumMessageResponse
    {
        /// <summary>
        /// Unique identifier of the message.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Text of the forum message/reply.
        /// </summary>
        public string? Text { get; set; }

        /// <summary>
        /// Time of creation.
        /// </summary>
        public DateTime? CreatedOn { get; set; }

        /// <summary>
        /// Defines is message pinned. This value defaults to false.
        /// </summary>
        public bool IsPinned { get; set; }

        /// <summary>
        /// The number of likes for the given message.
        /// </summary>
        public int LikesCount { get; set; }

        /// <summary>
        /// The number of replies for the given message.
        /// </summary>
        public int RepliesCount { get; set; }

        /// <summary>
        /// Defines is message liked by user that makes request.
        /// </summary>
        public bool IsLiked { get; set; }

        /// <summary>
        /// User object who sent message.
        /// </summary>
        public ForumUserResponse User { get; set; }

        /// <summary>
        /// List of the message attachments.
        /// </summary>
        public IEnumerable<AttachmentResponse> Attachments { get; set; }

        /// <summary>
        /// Time of Modification.
        /// </summary>
        public DateTime? ModifiedOn { get; set; }
    }
}

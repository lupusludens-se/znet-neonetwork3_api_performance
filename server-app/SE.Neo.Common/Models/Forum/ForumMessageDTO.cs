using SE.Neo.Common.Models.Shared;

namespace SE.Neo.Common.Models.Forum
{
    public class ForumMessageDTO
    {
        public int Id { get; set; }

        public int ForumId { get; set; }

        public string? Text { get; set; }

        public DateTime? CreatedOn { get; set; }

        public bool IsPinned { get; set; }

        public int LikesCount { get; set; }

        public int UserId { get; set; }

        public int? ParentMessageId { get; set; }

        public int RepliesCount { get; set; }

        public bool IsLiked { get; set; }

        public ForumUserDTO User { get; set; }

        public IEnumerable<AttachmentDTO> Attachments { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}

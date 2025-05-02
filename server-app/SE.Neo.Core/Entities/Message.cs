using SE.Neo.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("Message")]
    public class Message : BaseIdEntity
    {
        [Column("Message_Id")]
        public override int Id { get; set; }

        [Column("Message_Text")]
        [MaxLength(10000)]
        public string? Text { get; set; }

        [Column("Message_Is_Pinned")]
        public bool IsPinned { get; set; }

        [Column("Discussion_Id")]
        [ForeignKey("Discussion")]
        public int DiscussionId { get; set; }

        [Column("User_Id")]
        [ForeignKey("User")]
        public int UserId { get; set; }

        [Column("Parent_Message_Id")]
        [ForeignKey("Message")]
        public int? ParentMessageId { get; set; }

        public User User { get; set; }

        [Column("Status_Id")]
        public MessageStatus StatusId { get; set; } = MessageStatus.Active;

        public Discussion Discussion { get; set; }

        public ICollection<Attachment> Attachments { get; set; }

        public Message? ParentMessage { get; set; }

        public ICollection<Message> Messages { get; set; }

        public ICollection<MessageLike> MessageLikes { get; set; }
    }
}
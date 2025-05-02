using SE.Neo.Common.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SE.Neo.Core.Entities
{
    [Table("Attachment")]
    public class Attachment : BaseIdEntity
    {
        [Column("Attachment_Id")]
        public override int Id { get; set; }

        [Column("Attachment_Type")]
        public AttachmentType Type { get; set; }

        [MaxLength(4000)]
        public string Text { get; set; }

        public string Link { get; set; }

        [Column("Image_Name")]
        [ForeignKey("Image")]
        public string? ImageName { get; set; }

        public Blob? Image { get; set; }

        [Column("Message_Id")]
        [ForeignKey("Message")]
        public int MessageId { get; set; }

        public Message Message { get; set; }
    }
}
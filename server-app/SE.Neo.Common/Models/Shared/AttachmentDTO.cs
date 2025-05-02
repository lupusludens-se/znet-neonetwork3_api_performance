using SE.Neo.Common.Enums;
using SE.Neo.Common.Models.Media;

namespace SE.Neo.Common.Models.Shared
{
    public class AttachmentDTO
    {
        public int Id { get; set; }

        public int MessageId { get; set; }

        public string Text { get; set; }

        public string Link { get; set; }

        public AttachmentType Type { get; set; }

        public string? ImageName { get; set; }

        public BlobDTO? Image { get; set; }
    }
}
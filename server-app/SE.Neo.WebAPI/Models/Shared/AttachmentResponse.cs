using SE.Neo.Common.Enums;
using SE.Neo.WebAPI.Models.Media;

namespace SE.Neo.WebAPI.Models
{
    public class AttachmentResponse
    {
        public int Id { get; set; }

        public int MessageId { get; set; }

        public string Text { get; set; }

        public string Link { get; set; }

        public AttachmentType Type { get; set; }

        public string? ImageName { get; set; }

        public BlobResponse? Image { get; set; }
    }
}
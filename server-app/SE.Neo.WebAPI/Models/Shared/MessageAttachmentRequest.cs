using SE.Neo.Common.Enums;
using SE.Neo.WebAPI.Validation;

namespace SE.Neo.WebAPI.Models.Shared
{
    public class MessageAttachmentRequest
    {
        public int MessageId { get; set; }

        public string Text { get; set; }

        public string Link { get; set; }

        public AttachmentType Type { get; set; }

        [BlobNameExist]
        public string? ImageName { get; set; }
    }
}

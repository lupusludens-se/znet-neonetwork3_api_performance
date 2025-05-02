using SE.Neo.Core.Enums;

namespace SE.Neo.WebAPI.Models.Media
{
    public class BlobResponse
    {
        public string Name { get; set; }

        public BlobType BlobType { get; set; }

        public Uri Uri { get; set; }
    }
}

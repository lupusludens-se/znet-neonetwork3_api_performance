using SE.Neo.Common.Models.CMS;
using SE.Neo.Common.Models.Company;

namespace SE.Neo.WebAPI.Models.Initiative
{
    public class FileBlobRequest
    {
        public bool Overwrite { get; set; } = false;

        public string? BlobName { get; set; } = string.Empty;

        public string? NewFileName { get; set; } = string.Empty;
    }
}

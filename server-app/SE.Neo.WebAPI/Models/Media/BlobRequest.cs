using SE.Neo.Core.Enums;
using SE.Neo.WebAPI.Models.Initiative;
using SE.Neo.WebAPI.Validation;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Models.Media
{
    public class BlobRequest : FileBlobRequest
    {
        [Required, FormFileValid(nameof(BlobType))]
        public IFormFile File { get; set; }

        [Required, EnumExist(typeof(BlobType), "must contain a valid blob type Id")]
        public BlobType BlobType { get; set; }

        public bool IsLogoOnlyAllowed { get; set; } = false;
    }
}

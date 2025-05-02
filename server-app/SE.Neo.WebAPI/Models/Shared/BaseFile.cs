using SE.Neo.Common.Enums;
using SE.Neo.WebAPI.Validation;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Models.Shared
{
    public class BaseFile
    {
        [Required]
        public string ActualFileName { get; set; }
        [Required, EnumExist(typeof(FileType), "must contain valid file type")]

        public string ActualFileTitle { get; set; }
        public FileType Type { get; set; }
        [Required, EnumExist(typeof(FileExtension), "must contain valid file extension")]
        public FileExtension Extension { get; set; }
        [Required]
        public string Link { get; set; }
        [Required]
        public string BlobName { get; set; }
        [Required]
        public int Size { get; set; }
        [Required]
        public int Version { get; set; }
    }
}

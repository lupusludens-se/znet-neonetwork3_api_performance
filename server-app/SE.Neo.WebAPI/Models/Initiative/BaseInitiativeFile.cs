using SE.Neo.Common.Enums;
using SE.Neo.WebAPI.Models.User;
using SE.Neo.WebAPI.Validation;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Models.Initiative
{
    public class BaseInitiativeFile
    {
        [Required]
        public string ActualFileName { get; set; }
         
        public string ActualFileTitle { get; set; }
        [Required, EnumExist(typeof(FileType), "must contain valid file type")]
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
        public int Version  { get; set; }
        public int? CreatedByUserId { get; set; }
    }
}

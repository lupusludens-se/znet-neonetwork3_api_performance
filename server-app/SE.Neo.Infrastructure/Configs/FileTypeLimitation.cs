using System.ComponentModel.DataAnnotations;

namespace SE.Neo.Infrastructure.Configs
{
    public class FileTypeLimitation
    {
        [Required]
        public int Size { get; set; }
        [Required]
        public string[] Extensions { get; set; }
    }
}

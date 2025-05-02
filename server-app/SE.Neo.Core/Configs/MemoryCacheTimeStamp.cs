using System.ComponentModel.DataAnnotations;

namespace SE.Neo.Core.Configs
{
    public class MemoryCacheTimeStamp
    {
        [Required]
        public int Short { get; set; }

        [Required]
        public int Medium { get; set; }

        [Required]
        public int Long { get; set; }
    }
}
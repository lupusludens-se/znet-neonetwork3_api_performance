using System.ComponentModel.DataAnnotations;

namespace SE.Neo.Infrastructure.Configs
{
    public class WordPressConfig
    {
        [Required]
        public string ConnectionBaseUrl { get; set; }

        [Required]
        public string ConnectionApiUrl { get; set; }

        [Required]
        public string WebApiUserName { get; set; }

        [Required]
        public string WebApiUserPassword { get; set; }
    }
}
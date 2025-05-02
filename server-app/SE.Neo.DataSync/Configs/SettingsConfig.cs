using System.ComponentModel.DataAnnotations;

namespace SE.Neo.DataSync.Configs
{
    public class SettingsConfig
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        public string UserPassword { get; set; }

        [Required]
        public string AuthorityUrl { get; set; }

        [Required]
        public string ApiUrl { get; set; }

        [Required]
        public string AppClientId { get; set; }

        [Required]
        public string Scope { get; set; }
    }
}
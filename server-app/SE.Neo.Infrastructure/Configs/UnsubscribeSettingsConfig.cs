using System.ComponentModel.DataAnnotations;

namespace SE.Neo.Infrastructure.Configs
{
    public class UnsubscribeSettingsConfig
    {
        [Required]
        public string SecretKey { get; set; }

        [Required]
        public string IVKey { get; set; }

        [Required]
        public string UnsubscribeEmailUrlPattern { get; set; }
    }
}

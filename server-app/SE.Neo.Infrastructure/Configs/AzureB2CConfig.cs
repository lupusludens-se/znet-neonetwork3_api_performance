using System.ComponentModel.DataAnnotations;

namespace SE.Neo.Infrastructure.Configs
{
    public class AzureB2CConfig
    {
        [Required]
        public string Instance { get; set; }

        [Required]
        public string Domain { get; set; }

        [Required]
        public string ReserPasswordPolicyId { get; set; }

        [Required]
        public string AppClientId { get; set; }

        [Required]
        public string RedirectUrl { get; set; }
    }
}

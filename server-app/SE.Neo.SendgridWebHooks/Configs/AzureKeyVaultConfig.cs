using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebHooks.Configs
{
    public class AzureKeyVaultConfig
    {
        [Required]
        public string TenantId { get; set; }
        [Required]
        public string ClientId { get; set; }
        [Required]
        public string KeyVaultUrl { get; set; }
    }
}

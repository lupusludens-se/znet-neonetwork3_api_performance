using System.ComponentModel.DataAnnotations;

namespace SE.Neo.Infrastructure.Configs
{
    public class AzureStorageConfig
    {
        [Required]
        public Uri CdnEndpoint { get; set; }
        [Required]
        public string DefaultBlobContainer { get; set; }
        [Required]
        public string ConnectionString { get; set; }
    }
}

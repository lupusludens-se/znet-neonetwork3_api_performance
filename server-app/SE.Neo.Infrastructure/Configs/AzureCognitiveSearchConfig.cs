using System.ComponentModel.DataAnnotations;

namespace SE.Neo.Infrastructure.Configs
{
    public class AzureCognitiveSearchConfig
    {
        [Required]
        public string ApiKey { get; set; }

        [Required]
        public string ServiceName { get; set; }

        [Required]
        public string IndexName { get; set; }
    }
}

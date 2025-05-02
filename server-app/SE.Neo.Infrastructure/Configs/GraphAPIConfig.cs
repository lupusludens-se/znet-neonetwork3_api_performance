using System.ComponentModel.DataAnnotations;

namespace SE.Neo.Infrastructure.Configs
{
    public class GraphAPIConfig
    {
        [Required]
        public Guid TenantId { get; set; }

        [Required]
        public Guid AppClientId { get; set; }

        [Required]
        public string AppClientSecret { get; set; }

        [Required]
        public string Issuer { get; set; }
    }
}

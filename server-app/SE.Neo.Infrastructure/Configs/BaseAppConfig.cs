using System.ComponentModel.DataAnnotations;

namespace SE.Neo.Infrastructure.Configs
{
    public class BaseAppConfig
    {
        [Required]
        public string BaseAppUrlPattern { get; set; }
    }
}

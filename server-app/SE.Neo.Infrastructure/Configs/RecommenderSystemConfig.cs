using System.ComponentModel.DataAnnotations;

namespace SE.Neo.Infrastructure.Configs
{
    public class RecommenderSystemConfig
    {
        [Required]
        public string BaseConnectionUrl { get; set; }

        [Required]
        public string ProjectApiUrl { get; set; }
    }
}
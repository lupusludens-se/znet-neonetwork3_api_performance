using System.ComponentModel.DataAnnotations;

namespace SE.Neo.Infrastructure.Configs
{
    public class ScheduleDemoEmailConfig
    {
        [Required]
        public string SubjectForUser { get; set; }

        [Required]
        public string SubjectForAdmin { get; set; }

        [Required]
        public string To { get; set; }
    }
}

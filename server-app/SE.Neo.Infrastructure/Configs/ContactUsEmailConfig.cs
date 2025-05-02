using System.ComponentModel.DataAnnotations;

namespace SE.Neo.Infrastructure.Configs
{
    public class ContactUsEmailConfig
    {
        [Required]
        public string Subject { get; set; }

        [Required]
        public string To { get; set; }
    }
}
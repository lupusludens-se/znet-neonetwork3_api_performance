using System.ComponentModel.DataAnnotations;
namespace SE.Neo.Infrastructure.Configs
{
    public class EmailConfig
    {
        [Required]
        public string SendgridAPIKey { get; set; }

        [Required]
        public string SenderEmail { get; set; }

        [Required]
        public string SenderName { get; set; }

        [Required]
        public string ReplyTo { get; set; }

        [Required]
        public string SendgridActivityAPI { get; set; }

        [Required]
        public string CCAddressForUnDeliveredEmail { get; set; }

        [Required]
        public string ToAddressForUnDeliveredEmail { get; set; }
    }
}

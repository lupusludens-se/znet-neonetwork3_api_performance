using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebHooks.Configs
{
    public class SettingsConfig
    {
        //[Required]
        public string EventWebhookId { get; set; }

        [Required]
        public string SendgridWebhook { get; set; }

        [Required]
        public string ApiUrl { get; set; }

        [Required]
        public string SendgridAPIKey { get; set; }
    }
}
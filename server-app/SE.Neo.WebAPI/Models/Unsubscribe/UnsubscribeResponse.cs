using SE.Neo.Common.Enums;

namespace SE.Neo.WebAPI.Models.Unsubscribe
{
    public class UnsubscribeResponse
    {
        public string Email { get; set; }

        public string Message { get; set; }
        public EmailAlertFrequency? EmailPreference { get; set; }
    }
}

using SE.Neo.Common.Enums;

namespace SE.Neo.WebAPI.Models.UserProfile
{
    public class UnsubscribeRequest
    {
        public string Token { get; set; }

        public EmailAlertFrequency Frequency { get; set; }
    }
}
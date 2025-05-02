using SE.Neo.Common.Enums;

namespace SE.Neo.WebAPI.Models.EmailAlert
{
    public class EmailAlertItemRequest
    {
        public int Id { get; set; }
        public EmailAlertFrequency Frequency { get; set; }
    }
}

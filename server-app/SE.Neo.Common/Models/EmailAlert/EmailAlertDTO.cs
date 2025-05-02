using SE.Neo.Common.Enums;

namespace SE.Neo.Common.Models.EmailAlert
{
    public class EmailAlertDTO
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public EmailAlertCategory Category { get; set; }

        public EmailAlertFrequency Frequency { get; set; }
    }
}

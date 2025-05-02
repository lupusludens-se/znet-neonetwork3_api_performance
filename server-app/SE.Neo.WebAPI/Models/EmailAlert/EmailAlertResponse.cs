using SE.Neo.Common.Enums;

namespace SE.Neo.WebAPI.Models.EmailAlert
{
    public class EmailAlertResponse
    {
        /// <summary>
        /// Unique identifier of the email alert.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Title of the email alert.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Description of the email alert.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Frequency of the email alert.
        /// </summary>
        public EmailAlertFrequency Frequency { get; set; }
    }
}

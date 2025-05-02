using System.ComponentModel.DataAnnotations;

namespace SE.Neo.EmailAlertSender.Configs
{
    public class EmailAlertConfig
    {
        [Required]
        public string NewMessageAlertSingleSubject { get; set; }

        [Required]
        public string NewMessageAlertMultipleSubject { get; set; }

        [Required]
        public string ForumResponseAlertSingleSubject { get; set; }

        [Required]
        public string ForumResponseAlertMultipleSubject { get; set; }

        [Required]
        public string SummaryAlertSubject { get; set; }

        [Required]
        public string CreateEventInvitationAlertSubject { get; set; }

        [Required]
        public string UpdateEventInvitationAlertSubject { get; set; }

        [Required]
        public string EventReminderAlertSubject { get; set; }

        [Required]
        public string EventDeletedAlertSubject { get; set; }

        [Required]
        public string CompleteProfileReminderSubject { get; set; }

        [Required]
        public string UndeliveredMailSubject { get; set; }
    }
}
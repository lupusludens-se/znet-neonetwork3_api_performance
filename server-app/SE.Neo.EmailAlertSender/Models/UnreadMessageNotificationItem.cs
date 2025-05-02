namespace SE.Neo.EmailAlertSender.Models
{
    public class UnreadMessageNotificationItem
    {
        public int? MessageSenderUserId { get; set; }
        public int NotificationReceiverUserId { get; set; }
        public string NotificationDetails { get; set; }
        public string NotificationReceivedUserFirstName { get; set; }
        public DateTime? CreatedOn { get; set; }
    }
}

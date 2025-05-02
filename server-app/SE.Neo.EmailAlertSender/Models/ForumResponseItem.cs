namespace SE.Neo.EmailAlertSender.Models
{
    public class ForumResponseItem
    {
        public string ForumTopic { get; set; }
        public int ForumId { get; set; }
        public IEnumerable<ForumResponseMessageItem> Messages { get; set; }
    }
}
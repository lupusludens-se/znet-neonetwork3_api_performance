namespace SE.Neo.EmailAlertSender.Models
{
    public class NewMessageAlertItem
    {
        public string FirstName { get; set; }

        public string? MessageAuthor { get; set; }

        public string? MessageText { get; set; }

        public int NumberOfMessages { get; set; }
    }
}

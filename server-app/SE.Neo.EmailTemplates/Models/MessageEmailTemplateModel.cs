namespace SE.Neo.EmailTemplates.Models
{
    public class MessageEmailTemplateModel
    {
        public int MessageId { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string MessageAuthor { get; set; }
        public string AuthorProfileText { get; set; }

        public string MessageText { get; set; }

        public string MessageTime { get; set; }

        public string? AuthorProfileUrl { get; set; }

        public string? AuthorCompany { get; set; }

        public string ConversationSubject { get; set; }

        public int NumberOfDiscussionUsers { get; set; }

        public string replyLink { get; set; }

        public bool Attachment { get; set; }
    }
}

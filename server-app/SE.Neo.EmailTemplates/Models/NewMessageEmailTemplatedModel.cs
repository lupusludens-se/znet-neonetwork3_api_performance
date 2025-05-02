namespace SE.Neo.EmailTemplates.Models
{
    public class NewMessageEmailTemplatedModel : NotificationTemplateEmailModel
    {
        public override string TemplateName => "NewMessageEmailTemplate";

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public List<MessageEmailTemplateModel> messages { get; set; }

        public string Link { get; set; }

        public int NumberOfMessages { get; set; }

        public int? NumberOfConversations { get; set; }

        public string? attachmentLogoUrl { get; set; }
    }
}

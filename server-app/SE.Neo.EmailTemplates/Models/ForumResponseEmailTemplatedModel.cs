namespace SE.Neo.EmailTemplates.Models
{
    public class ForumResponseEmailTemplatedModel : NotificationTemplateEmailModel
    {
        public override string TemplateName => "ForumResponseEmailTemplate";

        public string FirstName { get; set; }

        public string ForumTopic { get; set; }

        public string? ResponseAuthor { get; set; }

        public string? ResponseText { get; set; }

        public string Link { get; set; }

        public int ResponseCount { get; set; }
    }
}

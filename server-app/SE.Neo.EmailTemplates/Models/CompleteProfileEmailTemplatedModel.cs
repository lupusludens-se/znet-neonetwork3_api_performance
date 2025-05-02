namespace SE.Neo.EmailTemplates.Models
{
    public class CompleteProfileEmailTemplatedModel : NotificationTemplateEmailModel
    {
        public override string TemplateName => "CompleteProfileEmailTemplate";

        public string FirstName { get; set; }

        public string Link { get; set; }
    }
}

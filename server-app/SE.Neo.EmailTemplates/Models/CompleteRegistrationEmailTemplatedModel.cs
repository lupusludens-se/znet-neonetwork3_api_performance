namespace SE.Neo.EmailTemplates.Models
{
    public class CompleteRegistrationEmailTemplatedModel : NotificationTemplateEmailModel
    {
        public override string TemplateName => "CompleteRegistrationEmailTemplate";

        public string FirstName { get; set; }

        public string Link { get; set; }
    }
}

namespace SE.Neo.EmailTemplates.Models
{
    public class ScheduleDemoToUserTemplateModel : NotificationTemplateEmailModel
    {
        public override string TemplateName => "ScheduleDemoToUser";
        public string Name { get; set; }

        public string DashboardLink { get; set; }

        public string Email { get; set; }
    }
}

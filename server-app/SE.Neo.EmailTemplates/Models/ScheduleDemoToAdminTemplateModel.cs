namespace SE.Neo.EmailTemplates.Models
{
    public class ScheduleDemoToAdminTemplateModel : NotificationTemplateEmailModel
    {
        public override string TemplateName => "ScheduleDemoToAdmin";

        public string Name { get; set; }

        public string Email { get; set; }

        public string Company { get; set; }

        public string BusinessType { get; set; }

        public string IamLookingFor { get; set; }

        public string Country { get; set; }

        public string JoiningInterestDetails { get; set; }

    }
}

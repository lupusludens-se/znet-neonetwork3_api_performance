namespace SE.Neo.EmailTemplates.Models
{
    public class UndeliveredMailTemplatedModel : NotificationTemplateEmailModel
    {
        public override string TemplateName => "UndeliveredMailTemplate";

        public List<UndeliveredData> messages { get; set; } = new List<UndeliveredData>();
    }

    public class UndeliveredData
    {
        public string to_email { get; set; }

        public string from_email { get; set; }

        public string subject { get; set; }

        public string msg_id { get; set; }

        public DateTime last_event_time { get; set; }
    }
}

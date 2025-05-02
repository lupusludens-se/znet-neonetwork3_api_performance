namespace SE.Neo.EmailTemplates.Models
{
    public class SummaryEmailTemplatedModel : NotificationTemplateEmailModel
    {
        public override string TemplateName => "SummaryEmailTemplate";

        public string FirstName { get; set; }

        public string Link { get; set; }

        public List<SummaryEmailItem> Items { get; set; }
    }

    public class SummaryEmailItem
    {
        public string ItemTypeLogoUrl { get; set; }

        public string ItemTypeName { get; set; }

        public string ItemLink { get; set; }
        public int ItemId { get; set; }
        public EventDateInfo? EventDateInfo { get; set; }

        public string? EventDateLogoUrl { get; set; }

        public string? EventTimeLogoUrl { get; set; }

        public string MainTitle { get; set; }

        public string MainText { get; set; }

        public List<string> Tags { get; set; }

        public List<string> Regions { get; set; }

        public string? Company { get; set; }

        public int? CompanyId { get; set; }

        public bool IsDisplayedInPublicSite { get; set; } = false;
         
        public string? InitiativeStepName { get; set; }

        public DateTime? InitiativeModifiedDate { get; set; }

        public int? InitiativeCategoryId { get; set; }

    }

}

using SE.Neo.EmailTemplates.Models.BaseModel;

namespace SE.Neo.EmailTemplates.Models
{
    public abstract class NotificationTemplateEmailModel : BaseTemplatedEmailModel
    {
        public string LogoUrl { get; set; }

        public string RegularFontLink { get; set; }

        public string LightFontLink { get; set; }

        public string BoldFontLink { get; set; }
    }
}

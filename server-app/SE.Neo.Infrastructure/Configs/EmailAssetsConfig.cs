using System.ComponentModel.DataAnnotations;

namespace SE.Neo.Infrastructure.Configs
{
    public class EmailAssetsConfig
    {
        [Required]
        public string AssetsBlobContainer { get; set; }
        [Required]
        public string NeoLogo { get; set; }
        [Required]
        public string NeoBlueLogo { get; set; }
        [Required]
        public string ZeigoLogo { get; set; }
        [Required]
        public string EventsDate { get; set; }
        [Required]
        public string EventsTime { get; set; }
        [Required]
        public string EventInvitationTimer { get; set; }
        [Required]
        public string EventsLogo { get; set; }
        [Required]
        public string ForumLogo { get; set; }
        [Required]
        public string LearnLogo { get; set; }
        [Required]
        public string NewsLogo { get; set; }
        [Required]
        public string ProjectsLogo { get; set; }
        [Required]
        public string InitiativeLogo { get; set; }
        [Required]
        public string ToolsLogo { get; set; }
        [Required]
        public string BoldFont { get; set; }
        [Required]
        public string LightFont { get; set; }
        [Required]
        public string RegularFont { get; set; }
        [Required]
        public string AttachmentLink { get; set; }

        public List<string> Logos
        {
            get
            {
                return new List<string>
                {
                    NeoLogo,
                    EventsDate,
                    EventsTime,
                    EventInvitationTimer,
                    EventsLogo,
                    ForumLogo,
                    LearnLogo,
                    NewsLogo,
                    ProjectsLogo,
                    ToolsLogo,
                    ZeigoLogo,
                    InitiativeLogo,
                    AttachmentLink
                };
            }
        }
    }
}

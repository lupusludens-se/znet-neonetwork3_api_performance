using SE.Neo.WebAPI.Enums;
using SE.Neo.WebAPI.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Models.Announcement
{
    public class AnnouncementRequest
    {
        [StringLength(1024, MinimumLength = 1)]
        public string? BackgroundImageName { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [Required]
        [StringLength(21)]
        public string ButtonText { get; set; }

        [Required]
        [UrlCustom, StringLength(2048)]
        public string ButtonUrl { get; set; }

        [Required]
        public bool IsActive { get; set; }

        [Required, EnumExist(typeof(AnnouncementAudienceType), "must contain valid announcement audience type id")]
        [DefaultValue(AnnouncementAudienceType.All)]
        public AnnouncementAudienceType AudienceId { get; set; }
    }
}

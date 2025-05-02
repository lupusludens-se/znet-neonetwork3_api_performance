using SE.Neo.Common.Models.Media;

namespace SE.Neo.Common.Models.Announcement
{
    public class AnnouncementDTO
    {
        public int Id { get; set; }

        public string? BackgroundImageName { get; set; }

        public BlobDTO? BackgroundImage { get; set; }

        public string Name { get; set; }

        public string ButtonText { get; set; }

        public string ButtonUrl { get; set; }

        public bool IsActive { get; set; }

        public int AudienceId { get; set; }

        public string Audience { get; set; }
    }
}

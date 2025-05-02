using SE.Neo.WebAPI.Models.Media;

namespace SE.Neo.WebAPI.Models.Announcement
{
    public class AnnouncementResponse
    {
        public int Id { get; set; }

        public string BackgroundImageName { get; set; }

        public BlobResponse BackgroundImage { get; set; }

        public string Name { get; set; }

        public string ButtonText { get; set; }

        public string ButtonUrl { get; set; }

        public bool IsActive { get; set; }

        public int AudienceId { get; set; }

        public string Audience { get; set; }
    }
}

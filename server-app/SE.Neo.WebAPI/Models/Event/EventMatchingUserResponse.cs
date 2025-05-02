using SE.Neo.WebAPI.Models.Media;

namespace SE.Neo.WebAPI.Models.Event
{
    public class EventMatchingUserResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
        public BlobResponse? Image { get; set; }
        public bool IsMatching { get; set; }
        public bool IsInvited { get; set; }
    }
}

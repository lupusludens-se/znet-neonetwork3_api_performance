using SE.Neo.Common.Models.Media;

namespace SE.Neo.Common.Models.Event
{
    public class EventMatchingUserDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
        public BlobDTO? Image { get; set; }
        public bool IsInvited { get; set; }
        public bool IsMatching { get; set; }
    }
}

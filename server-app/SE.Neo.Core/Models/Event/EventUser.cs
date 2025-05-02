using SE.Neo.Core.Entities;

namespace SE.Neo.Core.Models.Event
{
    public class EventUser
    {
        public int Id { get; set; }
        public bool IsInvited { get; set; }
        public bool IsMatching { get; set; }
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Blob? Image { get; set; }
        public string Company { get; set; }
    }
}

using SE.Neo.Common.Enums;

namespace SE.Neo.Common.Models.Event
{
    public class EventLinkDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Url { get; set; }

        public EventLinkType Type { get; set; }
    }
}

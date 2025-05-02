using SE.Neo.Common.Models.Shared;

namespace SE.Neo.Common.Models.Event
{
    public class EventsFilter : BaseSearchFilterModel
    {
        public DateTimeOffset? From { get; set; }

        public DateTimeOffset? To { get; set; }

        public bool HighlightedOnly { get; set; }
    }
}

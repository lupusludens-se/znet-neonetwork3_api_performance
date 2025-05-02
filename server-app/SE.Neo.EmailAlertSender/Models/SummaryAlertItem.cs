using SE.Neo.Common.Enums;
using TimeZone = SE.Neo.Core.Entities.TimeZone;

namespace SE.Neo.EmailAlertSender.Models
{
    public class SummaryAlertItem
    {
        public string ItemTypeLogoUrl { get; set; }

        public string ItemTypeName { get; set; }

        public EventAlertDateInfo? EventAlertDateInfo { get; set; }

        public string? EventDateLogoUrl { get; set; }

        public string? EventTimeLogoUrl { get; set; }

        public EventLocationType? EventLocationType { get; set; }

        public string MainTitle { get; set; }

        public string MainText { get; set; }

        public IEnumerable<string> Tags { get; set; }

        public IEnumerable<string> Regions { get; set; }

        public int ItemId { get; set; }

        public TimeZone? EventTimeZone { get; set; }

        public bool IsDisplayedInPublicSite { get; set; } = false;

    }
}
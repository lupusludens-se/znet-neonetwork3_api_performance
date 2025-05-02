namespace SE.Neo.WebAPI.Models.Event
{
    public class EventOccurrenceResponse
    {
        /// <summary>
        /// Unique identifier of the event occurrence.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Start date and time for event occurrence.
        /// </summary>
        public DateTime FromDate { get; set; }

        /// <summary>
        /// End date and time for event occurrence.
        /// </summary>
        public DateTime ToDate { get; set; }

        /// <summary>
        /// Standard Name for event occurrence timezone (including DST)
        /// </summary>
        public string TimeZoneName { get; set; }

        /// <summary>
        /// Abbreviation for event occurrence timezone (including DST)
        /// </summary>
        public string TimeZoneAbbr { get; set; }

        /// <summary>
        /// UTC offset for event occurrence timezone (including DST)
        /// </summary>
        public double TimeZoneUtcOffset { get; set; }
    }
}

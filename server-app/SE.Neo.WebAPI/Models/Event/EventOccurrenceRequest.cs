using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Models.Event
{
    public class EventOccurrenceRequest
    {
        /// <summary>
        /// Start date and time for event occurrence.
        /// </summary>
        [Required]
        public DateTime FromDate { get; set; }

        /// <summary>
        /// End date and time for event occurrence.
        /// </summary>
        [Required]
        public DateTime ToDate { get; set; }
    }
}

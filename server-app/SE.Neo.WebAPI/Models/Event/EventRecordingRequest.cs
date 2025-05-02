using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Models.Event
{
    public class EventRecordingRequest
    {
        /// <summary>
        /// Hyperlink.
        /// </summary>
        [Required]
        public string Url { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Models.Event
{
    public class EventLinkRequest
    {
        /// <summary>
        /// Name of the link to display.
        /// </summary>
        [Required]
        public string Name { get; set; }

        /// <summary>
        /// Hyperlink.
        /// </summary>
        [Required]
        public string Url { get; set; }
    }
}

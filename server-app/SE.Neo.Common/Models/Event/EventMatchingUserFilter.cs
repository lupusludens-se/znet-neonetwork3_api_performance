using System.ComponentModel.DataAnnotations;

namespace SE.Neo.Common.Models.Event
{
    public class EventMatchingUserFilter : PaginationModel
    {
        /// <summary>
        /// Example value: regionids=5,6&amp;statusids=1&amp;regionids=1,2&amp;categoryids=1
        /// </summary>
        [Required]
        public string MatchBy { get; set; }
        public string? Search { get; set; }
    }
}

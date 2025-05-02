namespace SE.Neo.Common.Models.Shared
{
    /// <summary>
    /// Search Filter
    /// </summary>
    public class BaseSearchFilterModel : ExpandOrderModel
    {
        /// <summary>
        /// Text used to perform search
        /// </summary>
        public string? Search { get; set; }

        /// <summary>
        /// Map the filter parameters
        /// </summary>
        public string? FilterBy { get; set; }

        /// <summary>
        /// Get random list with specific length.
        /// </summary>
        public int? Random { get; set; }
    }
}

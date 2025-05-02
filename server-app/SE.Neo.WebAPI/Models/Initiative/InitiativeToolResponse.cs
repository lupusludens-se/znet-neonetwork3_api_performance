using SE.Neo.WebAPI.Models.Media;

namespace SE.Neo.WebAPI.Models.Initiative
{
    public class InitiativeToolResponse
    {
        /// <summary>
        /// Unique identifier of the Tool.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The tool's title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// The tool's description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Hyperlink for main image of the tool.
        /// </summary>
        public BlobResponse? ImageUrl { get; set; }

        /// <summary>
        /// Is tool new
        /// </summary>
        public bool? IsNew { get; set; }
    }
}

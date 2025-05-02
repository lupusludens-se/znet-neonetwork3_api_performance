using SE.Neo.Common.Models.Media;

namespace SE.Neo.Common.Models.Initiative
{
    public class ToolForInitiativeDTO
    {
        /// <summary>
        /// Id of the Tool.
        /// </summary>
        public int Id { get; set; }
        public int InitiativeId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public BlobDTO? ImageUrl { get; set; }
        public bool? IsNew { get; set; } = false;
    }
}

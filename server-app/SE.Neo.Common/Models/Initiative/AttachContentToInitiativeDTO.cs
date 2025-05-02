using SE.Neo.Common.Enums;

namespace SE.Neo.Common.Models.Initiative
{
    /// <summary>
    /// 
    /// </summary>
    public class AttachContentToInitiativeDTO
    {
        /// <summary>
        /// Primary key of the content id
        /// </summary>
        public int ContentId { get; set; }

        /// <summary>
        /// Content Type
        /// </summary>
        public InitiativeModules ContentType { get; set; }

        /// <summary>
        /// List of Initiative Ids
        /// </summary>

        public List<int> InitiativeIds { get; set; } = new List<int>();
    }
}

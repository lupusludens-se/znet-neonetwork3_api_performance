using SE.Neo.Common.Enums;
using SE.Neo.Common.Models.User;

namespace SE.Neo.Common.Models.Initiative
{
    public class InitiativeFileDTO
    {
        /// <summary>
        /// Id of the initiative file item.
        /// </summary>
        public int Id { get; set; }
        public int InitiativeId { get; set; }
        public string ActualFileName { get; set; }
        public string ActualFileTitle { get; set; }
        public string Name { get; set; }
        public FileType Type { get; set; }
        public FileExtension Extension { get; set; }
        public string Link { get; set; }
        public DateTime? CreatedOn { get; set; }
        public DateTime? ModifiedOn { get; set; }
        public int Size { get; set; }
        public int Version { get; set; }
        public int? CreatedByUserId { get; set; }
        public UserDTO CreatedByUser { get; set; }
        public int? UpdatedByUserId { get; set; }
        public UserDTO UpdatedByUser { get; set; }
    }
}

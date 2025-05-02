using SE.Neo.Common.Enums;

namespace SE.Neo.Common.Models.Company
{
    public class CompanyFileDTO
    {
        /// <summary>
        /// Id of the Company file item.
        /// </summary>
        public int Id { get; set; }
        public int CompanyId { get; set; }
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
        public bool IsPrivate { get; set; }

        public string ModifiedBy { get; set; }
    }
}

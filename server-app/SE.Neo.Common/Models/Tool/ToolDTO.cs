using SE.Neo.Common.Models.Company;
using SE.Neo.Common.Models.Media;
using SE.Neo.Common.Models.Shared;

namespace SE.Neo.Common.Models.Tool
{
    public class ToolDTO
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ToolUrl { get; set; }
        public bool IsActive { get; set; }
        public int? ToolType { get; set; }
        public string? IconName { get; set; }
        public BlobDTO? Icon { get; set; }
        public bool? IsPinned { get; set; }
        public int ToolHeight { get; set; }
        public List<RoleDTO> Roles { get; set; }
        public List<CompanyDTO>? Companies { get; set; }
        public List<ToolPinnedDTO>? Pinned { get; set; }
    }
}
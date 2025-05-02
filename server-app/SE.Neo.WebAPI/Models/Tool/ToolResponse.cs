using SE.Neo.Common.Enums;
using SE.Neo.WebAPI.Models.Company;
using SE.Neo.WebAPI.Models.Media;
using SE.Neo.WebAPI.Models.Role;

namespace SE.Neo.WebAPI.Models.Tool
{
    public class ToolResponse
    {
        public int? Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string? IconName { get; set; }
        public BlobResponse? Icon { get; set; }
        public string ToolUrl { get; set; }
        public bool IsActive { get; set; }
        public bool? IsPinned { get; set; }
        public int ToolHeight { get; set; }
        public ToolType ToolType { get; set; }
        public List<RoleResponse> Roles { get; set; }
        public List<CompanyResponse>? Companies { get; set; }
    }
}

using SE.Neo.WebAPI.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Models.Tool
{
    public class ToolRequest
    {
        [StringLength(250, MinimumLength = 2), Required, ToolTitleUnique]
        public string Title { get; set; }

        [StringLength(4000, MinimumLength = 2), Required]
        public string Description { get; set; }

        [UrlCustom, Required]
        public string ToolUrl { get; set; }

        [Required, StringLength(1024, MinimumLength = 1)]
        public string? IconName { get; set; }

        [Range(400, 1500, ErrorMessage = "Tool height must be within 400-1500 pixels")]
        public int ToolHeight { get; set; }

        [DefaultValue("true")]
        public bool IsActive { get; set; }

        [RoleIdListExist, NotEmptyIfEmpty(nameof(CompanyIds))]
        public IEnumerable<int>? RoleIds { get; set; }

        [CompanyIdListExist, NotEmptyIfEmpty(nameof(RoleIds))]
        public IEnumerable<int>? CompanyIds { get; set; }
    }
}

using SE.Neo.Core.Enums;
using SE.Neo.WebAPI.Models.CMS;
using SE.Neo.WebAPI.Models.Shared;
using SE.Neo.WebAPI.Validation;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Models.Project
{
    public class ProjectRequest
    {
        [Required, StringLength(100)]
        public string Title { get; set; }

        [RequiredIfNot(nameof(StatusId), ProjectStatus.Draft), StringLength(200)]
        public string? SubTitle { get; set; }

        [RequiredIfNot(nameof(StatusId), ProjectStatus.Draft), MaxWordAttributes(8000, ErrorMessage = "There are too many characters in {0}.")]
        public string? Opportunity { get; set; }

        [RequiredIfNot(nameof(StatusId), ProjectStatus.Draft), MaxWordAttributes(4000, ErrorMessage = "There are too many characters in {0}.")]
        public string? OpportunityText { get; set; }

        [RequiredIfNot(nameof(StatusId), ProjectStatus.Draft), MaxWordAttributes(8000, ErrorMessage = "There are too many characters in {0}.")]
        public string? Description { get; set; }

        [RequiredIfNot(nameof(StatusId), ProjectStatus.Draft), MaxWordAttributes(4000, ErrorMessage = "There are too many characters in {0}.")]
        public string? DescriptionText { get; set; }

        [Required, CategoryIdExist]
        public int CategoryId { get; set; }

        [Required, EnumExist(typeof(ProjectStatus), "must contain valid project status id")]
        public ProjectStatus StatusId { get; set; }

        public bool IsPinned { get; set; }

        [UserIdExist]
        public int? OwnerId { get; set; }

        [TechnologyRelateToCategory(nameof(CategoryId))]
        public IEnumerable<TechnologyRequest>? Technologies { get; set; }

        [Required]
        public IEnumerable<RegionRequest> Regions { get; set; }

        public int? CompanyId { get; set; }
    }

}

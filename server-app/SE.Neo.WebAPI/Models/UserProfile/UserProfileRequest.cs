using SE.Neo.Core.Enums;
using SE.Neo.WebAPI.Models.CMS;
using SE.Neo.WebAPI.Models.Shared;
using SE.Neo.WebAPI.Validation;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Models.UserProfile
{
    public class UserProfileRequest
    {
        [UserIdExistAndUnique, Required]
        public int UserId { get; set; }

        [StringLength(250, MinimumLength = 2), Required]
        public string JobTitle { get; set; }

        [LinkedInUrl]
        [StringLength(2048)]
        public string? LinkedInUrl { get; set; }

        public string? About { get; set; }

        public bool AcceptWelcomeSeriesEmail { get; set; }

        [StateIdExist]
        public int? StateId { get; set; }

        [EnumExist(typeof(Responsibility), "must be a valid field id")]
        public Responsibility? ResponsibilityId { get; set; }

        public IEnumerable<CategoryRequest>? Categories { get; set; }

        public IEnumerable<RegionRequest>? Regions { get; set; }

        public IEnumerable<UrlLinkModel>? UrlLinks { get; set; }
        public IEnumerable<SkillsByCategoryModel>? SkillsByCategory { get; set; }
    }
}
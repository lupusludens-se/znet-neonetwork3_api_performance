using SE.Neo.Core.Enums;
using SE.Neo.WebAPI.Models.CMS;
using SE.Neo.WebAPI.Models.Shared;
using SE.Neo.WebAPI.Validation;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Models.Company
{
    public class CompanyRequest
    {
        [StringLength(250, MinimumLength = 2), Required, CompanyNameUnique, CompanyName]
        public string Name { get; set; }

        [Required, EnumExist(typeof(CompanyStatus), "must contain valid company status id")]
        public CompanyStatus StatusId { get; set; }

        [Required, EnumExist(typeof(Common.Enums.CompanyType), "must contain valid company role id")]
        public Common.Enums.CompanyType TypeId { get; set; }

        [StringLength(1024, MinimumLength = 1)]
        public string? ImageLogo { get; set; }

        [UrlCustom]
        [StringLength(2048)]
        public string? CompanyUrl { get; set; }

        [LinkedInUrl]
        [StringLength(2048)]
        public string? LinkedInUrl { get; set; }

        public string? About { get; set; }

        public string? AboutText { get; set; }

        [CompanyMDM]
        public string? MDMKey { get; set; }

        [Required, IndustryIdExist]
        public int IndustryId { get; set; }

        [Required, CountryIdExist]
        public int CountryId { get; set; }
        public IEnumerable<UrlLinkModel>? UrlLinks { get; set; }

        public IEnumerable<OffsitePPAsRequest>? OffsitePPAs { get; set; }

        public IEnumerable<CategoryRequest>? Categories { get; set; }
        public int? CompanyId { get; set; }
    }
}
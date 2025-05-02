using SE.Neo.WebAPI.Validation;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Models.Company
{
    public class CompanyAnnouncementCreateOrUpdateRequest
    {
        [Required]
        [ValidatePlainTextLength(100)]
        public string Title { get; set; }

        [Required]
        public string Link { get; set; }

        [Required]
        [Range(1, 3)]
        public int ScaleId { get; set; }

        [Required]
        public List<int> RegionIds { get; set; }

        [Required]
        public int CompanyId { get; set; }

    }
}

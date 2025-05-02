using SE.Neo.WebAPI.Validation;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Models.Initiative
{
    public class InitiativeCreateOrUpdateRequest
    {
        [Required]
        [ValidatePlainTextLength(100)]
        public string Title { get; set; }

        [Required]
        [Range(1, 3)]
        public int ScaleId { get; set; }

        [Required]
        public List<int> RegionIds { get; set; }

        [Required]
        public int ProjectTypeId { get; set; }

        public List<int> CollaboratorIds { get; set; }

    }
}

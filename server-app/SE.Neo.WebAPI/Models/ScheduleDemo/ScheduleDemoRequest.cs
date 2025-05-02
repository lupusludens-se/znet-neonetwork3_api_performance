using SE.Neo.WebAPI.Validation;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Models.ScheduleDemo
{
    public class ScheduleDemoRequest
    {
        [Required, StringLength(250, MinimumLength = 2)]
        public string Name { get; set; }

        [Required, MaxLength(250), EmailAddressCustom]
        public string Email { get; set; }

        [Required, StringLength(250, MinimumLength = 2)]
        public string Company { get; set; }

        [Required]
        public string BusinessType { get; set; }

        [Required]
        public string IamLookingFor { get; set; }

        [Required, CountryIdExist]
        public string Country { get; set; }

        [StringLength(200)]
        public string? JoiningInterestDetails { get; set; }

    }
}

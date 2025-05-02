using SE.Neo.WebAPI.Validation;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Models.Shared
{
    public class ContactUsRequest
    {
        [Required, StringLength(250, MinimumLength = 2)]
        public string FirstName { get; set; }

        [Required, StringLength(250, MinimumLength = 2)]
        public string LastName { get; set; }

        [Required, MaxLength(250), EmailAddressCustom]
        public string Email { get; set; }

        [Required, StringLength(250, MinimumLength = 2)]
        public string Company { get; set; }

        [Required]
        public string Message { get; set; }
    }
}
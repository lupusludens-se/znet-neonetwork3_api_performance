using SE.Neo.WebAPI.Validation;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Models.User
{
    public class UserPendingAddRequest : UserPendingRequest
    {
        [Required, MinLength(1), MaxLength(250), CompanyName]
        public string CompanyName { get; set; }

        [Required]
        public string TimeZoneClientId { get; set; }
    }

}

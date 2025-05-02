using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Models.User
{
    public class UserPendingEditRequest : UserPendingRequest
    {
        public int? CompanyId { get; set; }

        [Required]
        public string CompanyName { get; set; }
    }
}

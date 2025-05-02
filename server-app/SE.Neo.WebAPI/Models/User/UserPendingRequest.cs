using SE.Neo.Common.Enums;
using SE.Neo.Core.Enums;
using SE.Neo.WebAPI.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Models.User
{
    public abstract class UserPendingRequest
    {
        [Required, MinLength(2), MaxLength(64), Name]
        public string FirstName { get; set; }

        [Required, MinLength(2), MaxLength(64), Name]
        public string LastName { get; set; }

        [Required, MaxLength(70), EmailAddressCustom, UniqueEmail]
        public string Email { get; set; }

        [Required, DisplayName("Role"), ValidateUserRole]
        public RoleType RoleId { get; set; }

        [Required, CountryIdExist]
        public int CountryId { get; set; }

        [Required, EnumExist(typeof(HeardVia), "must be a valid field id")]
        [DefaultValue(HeardVia.Other)]
        public int HeardViaId { get; set; }
        public string? AdminComments { get; set; }
        public string? JoiningInterestDetails { get; set; }
    }
}

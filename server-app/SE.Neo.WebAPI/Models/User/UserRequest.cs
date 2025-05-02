using SE.Neo.WebAPI.Models.EmailAlert;
using SE.Neo.WebAPI.Models.Permission;
using SE.Neo.WebAPI.Models.Role;
using SE.Neo.WebAPI.Validation;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Models.User
{
    public class UserRequest
    {
        [StringLength(64, MinimumLength = 2), Required, Name]
        public string FirstName { get; set; }

        [StringLength(64, MinimumLength = 2), Required, Name]
        public string LastName { get; set; }

        [Required, MaxLength(70), EmailAddressCustom, UniqueEmail, UserEmailUnique]
        public string Email { get; set; }

        [Required, EmailAddressCustom, UniqueEmail, UserNameUnique]
        [StringLength(70)]
        public string Username { get; set; }

        [Required, EnumExist(typeof(Core.Enums.UserStatus), "must contain valid user status id")]
        public Core.Enums.UserStatus StatusId { get; set; }

        [Required, CompanyIdExist]
        public int CompanyId { get; set; }

        [StringLength(1024, MinimumLength = 1)]
        public string? ImageName { get; set; }

        [Required, CountryIdExist]
        public int CountryId { get; set; }

        [TimeZoneIdExist]
        public int? TimeZoneId { get; set; }

        [Required, EnumExist(typeof(Core.Enums.HeardVia), "must be a valid field id")]
        [DefaultValue(Core.Enums.HeardVia.Other)]
        public Core.Enums.HeardVia HeardViaId { get; set; }

        public IEnumerable<RoleRequest>? Roles { get; set; }

        public IEnumerable<PermissionRequest>? Permissions { get; set; }

        public bool? IsPrivateUser { get; set; }

        public string? AdminComments { get; set; }

        public EmailAlertRequest? EmailAlerts { get; set; }
    }
}
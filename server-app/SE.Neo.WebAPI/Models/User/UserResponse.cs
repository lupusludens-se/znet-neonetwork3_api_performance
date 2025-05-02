using SE.Neo.WebAPI.Models.Company;
using SE.Neo.WebAPI.Models.Country;
using SE.Neo.WebAPI.Models.EmailAlert;
using SE.Neo.WebAPI.Models.Media;
using SE.Neo.WebAPI.Models.Permission;
using SE.Neo.WebAPI.Models.Role;
using SE.Neo.WebAPI.Models.Shared;
using SE.Neo.WebAPI.Models.UserProfile;

namespace SE.Neo.WebAPI.Models.User
{
    public class UserResponse
    {
        public int Id { get; set; }

        public UserProfileResponse? UserProfile { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public string Username { get; set; }

        public int StatusId { get; set; }

        public string StatusName { get; set; }

        public int CompanyId { get; set; }

        public CompanyResponse Company { get; set; }

        public string ImageName { get; set; }

        public BlobResponse Image { get; set; }

        public int CountryId { get; set; }

        public CountryResponse Country { get; set; }

        public int TimeZoneId { get; set; }

        public TimeZoneResponse TimeZone { get; set; }

        public int HeardViaId { get; set; }

        public string HeardViaName { get; set; }

        public DateTime? RequestDeleteDate { get; set; }

        public IEnumerable<RoleResponse> Roles { get; set; }

        public IEnumerable<PermissionResponse>? Permissions { get; set; }

        public bool? IsPrivateUser { get; set; }

        public string? AdminComments { get; set; }

        public string? ApprovedBy { get; set; }

        public IEnumerable<EmailAlertResponse>? EmailAlerts { get; set; }
    }
}
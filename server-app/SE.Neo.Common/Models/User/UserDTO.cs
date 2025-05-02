using SE.Neo.Common.Models.Company;
using SE.Neo.Common.Models.Media;
using SE.Neo.Common.Models.Shared;
using SE.Neo.Common.Models.UserProfile;

namespace SE.Neo.Common.Models.User
{
    public class UserDTO
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }
        public string Username { get; set; }

        public int CompanyId { get; set; }

        public CompanyDTO Company { get; set; }

        public int StatusId { get; set; }

        public string StatusName { get; set; }

        public string? ImageName { get; set; }

        public BlobDTO? Image { get; set; }

        public int TimeZoneId { get; set; }

        public TimeZoneDTO TimeZone { get; set; }

        public int CountryId { get; set; }

        public CountryDTO Country { get; set; }

        public int HeardViaId { get; set; }

        public string HeardViaName { get; set; }

        public string AzureId { get; set; }

        public DateTime? RequestDeleteDate { get; set; }

        public IEnumerable<RoleDTO> Roles { get; set; }

        public IEnumerable<PermissionDTO> Permissions { get; set; }

        public UserProfileDTO? UserProfile { get; set; }
        /// <summary>
        /// Property is set to true or false in case of Admin or Internal SE users, otherwise its null.
        /// </summary>
        public bool? IsPrivateUser { get; set; }

        public string? AdminComments { get; set; }

        public string? ApprovedBy { get; set; }

        public bool? IsFollowed { get; set; }

    }
}
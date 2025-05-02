using SE.Neo.Common.Models.Company;
using SE.Neo.Common.Models.Shared;

namespace SE.Neo.Common.Models.User
{
    public class UserPendingDTO
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public bool IsDenied { get; set; }

        public int RoleId { get; set; }

        public RoleDTO Role { get; set; }

        public string CompanyName { get; set; }

        public int? CompanyId { get; set; }

        public CompanyDTO? Company { get; set; }

        public int CountryId { get; set; }

        public CountryDTO Country { get; set; }

        public string TimeZoneClientId { get; set; }

        public int TimeZoneId { get; set; }

        public TimeZoneDTO TimeZoneDTO { get; set; }

        public int HeardViaId { get; set; }

        public string HeardViaName { get; set; }

        public DateTime CreatedDate { get; set; }

        public string AzureId { get; set; }

        public string? AdminComments { get; set; }

        public string? JoiningInterestDetails { get; set; }
    }
}

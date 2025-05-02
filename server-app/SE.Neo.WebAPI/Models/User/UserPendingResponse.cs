using SE.Neo.WebAPI.Models.Company;
using SE.Neo.WebAPI.Models.Role;

namespace SE.Neo.WebAPI.Models.User
{
    public class UserPendingResponse
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public bool IsDenied { get; set; }

        public int RoleId { get; set; }

        public RoleResponse Role { get; set; }

        public string CompanyName { get; set; }

        public int? CompanyId { get; set; }

        public CompanyResponse? Company { get; set; }
    }
}

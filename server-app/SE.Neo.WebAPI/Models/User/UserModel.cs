using SE.Neo.Common.Models.User;
using SE.Neo.Core.Enums;

namespace SE.Neo.WebAPI.Models.User
{
    public class UserModel : BaseUserModel
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Username { get; set; }

        public UserStatus Status { get; set; }

        public int CompanyId { get; set; }

        public List<int> RoleIds { get; set; }

        public List<int> PermissionIds { get; set; }

        public List<int>? CMSRoleIds { get; set; }

        public bool HasUserProfile { get; set; }

        public string AdminComments { get; set; }
    }
}

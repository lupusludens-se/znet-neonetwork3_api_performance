using SE.Neo.WebAPI.Models.Role;

namespace SE.Neo.WebAPI.Models.Permission
{
    public class PermissionResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<RoleResponse> Roles { get; set; }
    }
}
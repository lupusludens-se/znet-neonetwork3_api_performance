using SE.Neo.WebAPI.Models.Permission;

namespace SE.Neo.WebAPI.Models.Role
{
    public class RoleResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ToolTip { get; set; }
        public bool IsSpecial { get; set; }
        public IEnumerable<PermissionResponse> Permissions { get; set; }
    }
}
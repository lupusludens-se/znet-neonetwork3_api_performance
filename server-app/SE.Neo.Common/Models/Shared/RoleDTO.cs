namespace SE.Neo.Common.Models.Shared
{
    public class RoleDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public bool IsSpecial { get; set; }

        public string ToolTip { get; set; }

        public int? CMSRoleId { get; set; }

        public IEnumerable<PermissionDTO> Permissions { get; set; }
    }
}
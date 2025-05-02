namespace SE.Neo.Common.Models.Shared
{
    public class PermissionDTO
    {
        public int Id { get; set; }

        public string Name { get; set; }
        public IEnumerable<RoleDTO> Roles { get; set; }
    }
}
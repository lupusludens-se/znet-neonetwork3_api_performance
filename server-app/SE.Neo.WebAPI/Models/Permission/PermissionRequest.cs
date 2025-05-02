using SE.Neo.Core.Enums;
using SE.Neo.WebAPI.Validation;

namespace SE.Neo.WebAPI.Models.Permission
{
    public class PermissionRequest
    {
        [EnumExist(typeof(PermissionType), "must be a valid permission id")]
        public PermissionType Id { get; set; }
    }
}
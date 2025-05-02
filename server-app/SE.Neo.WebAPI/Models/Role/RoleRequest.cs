using SE.Neo.WebAPI.Validation;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Models.Role
{
    public class RoleRequest
    {
        [Required, RoleIdExist]
        public int Id { get; set; }
    }
}
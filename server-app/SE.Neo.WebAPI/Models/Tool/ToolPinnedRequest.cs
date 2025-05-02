using SE.Neo.WebAPI.Validation;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Models.Tool
{
    public class ToolPinnedRequest
    {
        [Required, ToolIdExist]
        public int ToolId { get; set; }
    }
}
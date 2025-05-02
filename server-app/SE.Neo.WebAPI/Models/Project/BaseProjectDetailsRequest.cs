using SE.Neo.Core.Enums;
using SE.Neo.WebAPI.Validation;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Models.Project
{
    public abstract class BaseProjectDetailsRequest
    {
        [Required, EnumExist(typeof(ProjectStatus), "must contain valid project status id")]
        public ProjectStatus StatusId { get; set; }
    }
}

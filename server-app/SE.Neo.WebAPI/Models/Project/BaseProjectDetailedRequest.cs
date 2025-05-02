
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Models.Project
{
    public abstract class BaseProjectDetailedRequest<T>
    {
        [Required]
        public abstract ProjectRequest Project { get; set; }

        [Required]
        public abstract T ProjectDetails { get; set; }
    }
}

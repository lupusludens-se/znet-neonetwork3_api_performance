using SE.Neo.WebAPI.Validation;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Models.Saved
{
    public class ProjectSavedRequest
    {
        /// <summary>
        /// Unique identifier of the project.
        /// </summary>
        [Required, ProjectIdIsActive]
        public int ProjectId { get; set; }
    }
}

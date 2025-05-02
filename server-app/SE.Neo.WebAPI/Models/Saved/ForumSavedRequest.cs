using SE.Neo.WebAPI.Validation;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Models.Saved
{
    public class ForumSavedRequest
    {
        /// <summary>
        /// Unique identifier of the forum.
        /// </summary>
        [Required, ForumIdExist]
        public int ForumId { get; set; }
    }
}

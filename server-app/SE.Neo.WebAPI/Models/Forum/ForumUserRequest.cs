using SE.Neo.WebAPI.Validation;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Models.Forum
{
    public class ForumUserRequest
    {
        /// <summary>
        /// Unique identifier of the user.
        /// </summary>
        [Required, UserIdExist]
        public int Id { get; set; }
    }
}

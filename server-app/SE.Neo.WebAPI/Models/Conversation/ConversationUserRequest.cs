using SE.Neo.WebAPI.Validation;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Models.Conversation
{
    public class ConversationUserRequest
    {
        /// <summary>
        /// Unique identifier of the user.
        /// </summary>
        [Required, UserIdExist]
        public int Id { get; set; }
    }
}

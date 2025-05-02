using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Models.Event
{
    public class EventModeratorRequest : IValidatableObject
    {
        /// <summary>
        /// Name of the moderator that not exist in the system.
        /// </summary>
        public string? Name { get; set; }

        /// <summary>
        /// Company name of the moderator that not exist in the system.
        /// </summary>
        public string? Company { get; set; }

        /// <summary>
        /// Unique identifier of the existing user.
        /// </summary>
        public int? UserId { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            var result = new List<ValidationResult>();

            if (UserId.HasValue && UserId > 0)
            {
                result.Add(ValidationResult.Success!);
            }
            else
            {
                if (string.IsNullOrEmpty(Name))
                {
                    result.Add(new ValidationResult("Must provide Name if Event Moderator has no userId."));
                }
                else
                {
                    result.Add(ValidationResult.Success!);
                }

            }
            return result;
        }
    }
}

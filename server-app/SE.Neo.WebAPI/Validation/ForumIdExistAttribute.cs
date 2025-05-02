using SE.Neo.Core.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Validation
{
    public class ForumIdExistAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            int? id = (int?)value;

            if (!id.HasValue)
            {
                return ValidationResult.Success!;
            }

            if (id.Value <= 0)
            {
                return new ValidationResult($"{validationContext.DisplayName} must be a valid forum id.");
            }

            IForumService service = (IForumService)validationContext.GetService(typeof(IForumService));

            var exist = service.IsForumExist(id.Value);
            return exist ? ValidationResult.Success! : new ValidationResult($"{validationContext.DisplayName} must be a valid forum id.");
        }
    }
}
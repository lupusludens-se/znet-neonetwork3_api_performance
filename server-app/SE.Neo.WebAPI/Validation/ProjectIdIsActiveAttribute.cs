using SE.Neo.Core.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Validation
{
    public class ProjectIdIsActive : ValidationAttribute
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
                return new ValidationResult($"{validationContext.DisplayName} must be a valid project id.");
            }

            IProjectService service = (IProjectService)validationContext.GetService(typeof(IProjectService));

            var exist = service.IsProjectActive(id.Value);
            return exist ? ValidationResult.Success! : new ValidationResult($"{validationContext.DisplayName} must be an active project id.");
        }
    }
}


using SE.Neo.Core.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Validation
{
    public class ProjectIdExistAttribute : ValidationAttribute
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

            var exist = service.IsProjectExist(id.Value);
            return exist ? ValidationResult.Success! : new ValidationResult($"{validationContext.DisplayName} must be a valid project id.");
        }
    }
}
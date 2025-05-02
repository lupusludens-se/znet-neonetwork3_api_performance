using SE.Neo.Core.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Validation
{
    public class BlobNameExistAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string? name = (string?)value;

            if (string.IsNullOrEmpty(name))
            {
                return ValidationResult.Success!;
            }

            IBlobService service = (IBlobService)validationContext.GetService(typeof(IBlobService));

            var exist = service.IsBlobExist(name);
            return exist ? ValidationResult.Success! : new ValidationResult($"{validationContext.DisplayName} must be a valid blob name.");
        }
    }
}
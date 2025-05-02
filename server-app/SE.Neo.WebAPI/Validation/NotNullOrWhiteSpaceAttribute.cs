using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Validation
{
    public class NotNullOrWhiteSpaceAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string? text = value as string;
            if (string.IsNullOrWhiteSpace(text))
            {
                return new ValidationResult($"The {validationContext.DisplayName} field should not be empty.");
            }
            return ValidationResult.Success;
        }
    }
}
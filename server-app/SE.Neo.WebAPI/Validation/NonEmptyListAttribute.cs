using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Validation
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class NonEmptyListAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success!;

            return value is IList list && list.Count > 0
                ? ValidationResult.Success!
                : new ValidationResult($"The {validationContext.DisplayName} should contain at least 1 element.");
        }
    }
}

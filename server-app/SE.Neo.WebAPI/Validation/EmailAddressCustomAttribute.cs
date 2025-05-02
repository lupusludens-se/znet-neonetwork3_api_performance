using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace SE.Neo.WebAPI.Validation
{
    public class EmailAddressCustomAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string email = value as string;
            if (string.IsNullOrEmpty(email))
                return ValidationResult.Success;

            var baseValidationResult = new EmailAddressAttribute().GetValidationResult(email, validationContext);

            if (baseValidationResult != ValidationResult.Success)
                return baseValidationResult;

            Regex regex = new Regex(@"^\w+([-.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$");
            Match match = regex.Match(email);

            if (!match.Success)
                return new ValidationResult($"The {validationContext.DisplayName} field is not a valid e-mail address.");

            return ValidationResult.Success;
        }
    }
}
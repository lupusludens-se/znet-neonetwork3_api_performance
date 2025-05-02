using System.ComponentModel.DataAnnotations;
using System.Net;

namespace SE.Neo.WebAPI.Validation
{
    public class UrlCustomAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string? url = value as string;

            if (string.IsNullOrEmpty(url))
                return ValidationResult.Success;

            url = WebUtility.UrlDecode(url);

            if (url.StartsWith("http://", StringComparison.OrdinalIgnoreCase)
                || url.StartsWith("https://", StringComparison.OrdinalIgnoreCase)
                || url.StartsWith("ftp://", StringComparison.OrdinalIgnoreCase)
                || url.StartsWith("www.", StringComparison.OrdinalIgnoreCase)
                )
                return ValidationResult.Success;

            return new ValidationResult($"The {validationContext.DisplayName} field is not a valid fully - qualified http, https, or ftp URL.");
        }
    }
}
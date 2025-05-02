using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.RegularExpressions;

namespace SE.Neo.WebAPI.Validation
{
    public class LinkedInUrlAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string? url = value as string;

            if (string.IsNullOrEmpty(url))
                return ValidationResult.Success;

            url = WebUtility.UrlDecode(url);

            Regex regex = new Regex(@"^(http(s)?:\/\/)?([a-z]{2,3}\.)?linkedin\.com\/(pub\/|in\/|profile\/|company\/)");

            Match match = regex.Match(url);

            if (!match.Success)
                return new ValidationResult($"The {validationContext.DisplayName} field is not a valid LinkedIn url.");

            return ValidationResult.Success;
        }
    }
}
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.RegularExpressions;

namespace SE.Neo.WebAPI.Validation
{
    public class NameAttribute : ValidationAttribute
    {
        private static readonly Regex NameExcludeSymbolsRegex = new Regex(@"[\d!@#$%^&*()[\]:`~{}<>="";?/\\|+]");

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string name = value as string;
            if (string.IsNullOrEmpty(name))
            {
                return ValidationResult.Success;
            }
            name = WebUtility.UrlDecode(name);

            Match match = NameExcludeSymbolsRegex.Match(name);
            if (match.Success)
            {
                return new ValidationResult($"{validationContext.DisplayName} must be a valid name. It can't include any digit and those characters !@#$%^&*()[]:`~{{}}<>=\";?/|+");
            }

            return ValidationResult.Success;
        }
    }
}
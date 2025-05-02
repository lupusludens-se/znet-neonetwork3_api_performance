using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Text.RegularExpressions;

namespace SE.Neo.WebAPI.Validation
{
    public class CompanyNameAttribute : ValidationAttribute
    {
        private static readonly Regex NameExcludeSymbolsRegex = new Regex(@"[@#$%^*[\]:`~{}<>="";/\\|]");

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string name = value as string;
            if (string.IsNullOrEmpty(name))
            {
                return ValidationResult.Success;
            }
            name = WebUtility.UrlDecode(name);

            var urlValidationResult = (new UrlCustomAttribute().GetValidationResult(name, validationContext) != ValidationResult.Success);

            Match match = NameExcludeSymbolsRegex.Match(name);
            if (match.Success & urlValidationResult)
            {
                return new ValidationResult($"{validationContext.DisplayName} must be a valid not URL name. It can't include those characters @#$%^*[]:`~{{}}<>=\";/|");
            }

            return ValidationResult.Success;
        }
    }
}
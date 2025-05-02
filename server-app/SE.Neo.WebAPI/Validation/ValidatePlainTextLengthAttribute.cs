using System.ComponentModel.DataAnnotations;
using System.Net;

namespace SE.Neo.WebAPI.Validation
{
    public class ValidatePlainTextLengthAttribute : ValidationAttribute
    {
        public ValidatePlainTextLengthAttribute(int maxLength)
        {
            if (maxLength == null)
            {
                throw new ArgumentNullException("maxLength");
            }
            MaxLength = maxLength;
        }

        public int MaxLength { get; private set; }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string? comment = value as string;
            if (!String.IsNullOrEmpty(comment))
            {
                comment = WebUtility.UrlDecode(comment);
                comment = WebUtility.HtmlDecode(comment);
            }

            if (comment.Length > 0 && comment.Length <= MaxLength)
            {
                return ValidationResult.Success;
            }
            else if (comment.Length > MaxLength)
            {
                return new ValidationResult($"The {validationContext.DisplayName} field has more than 1000 characters.");
            }
            else
            {
                return new ValidationResult($"{validationContext.DisplayName} is not a valid string.");
            }
        }
    }
}

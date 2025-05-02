using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Validation
{
    public class CompanyMDMAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string mdmKey = value as string;
            if (string.IsNullOrEmpty(mdmKey))
                return ValidationResult.Success;

            if (!mdmKey.ToLower().StartsWith("org-") && (!mdmKey.ToLower().StartsWith("mdm-")))
                return new ValidationResult($"The {validationContext.DisplayName} field should start with 'ORG- or MDM-'.");

            return ValidationResult.Success;
        }
    }
}
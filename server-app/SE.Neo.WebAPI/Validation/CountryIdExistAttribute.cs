using SE.Neo.Core.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Validation
{
    public class CountryIdExistAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var id = value as int?;
            if (id == null)
                return ValidationResult.Success;

            if (id.Value <= 0)
            {
                return new ValidationResult($"{validationContext.DisplayName} must be a valid country id.");
            }

            ICommonService service = (ICommonService)validationContext.GetService(typeof(ICommonService));

            var exist = service.IsCountryIdExistAsync(id.Value).Result;
            return exist ? ValidationResult.Success : new ValidationResult($"{validationContext.DisplayName} must be a valid country id.");
        }
    }
}
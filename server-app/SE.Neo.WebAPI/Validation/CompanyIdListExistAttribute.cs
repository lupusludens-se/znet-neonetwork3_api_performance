using SE.Neo.Core.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Validation
{
    public class CompanyIdListExistAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;
            var ids = value as IEnumerable<int>;
            if (ids == null)
                return new ValidationResult($"{validationContext.DisplayName} must contain valid company ids.");
            if (ids.Any(id => id <= 0))
            {
                return new ValidationResult($"{validationContext.DisplayName} must contain valid company ids.");
            }

            ICompanyService service = (ICompanyService)validationContext.GetService(typeof(ICompanyService));

            var exist = ids.All(id => service.IsCompanyIdExistAsync(id).Result);
            return exist ? ValidationResult.Success : new ValidationResult($"{validationContext.DisplayName} must contain valid company ids.");
        }
    }

}

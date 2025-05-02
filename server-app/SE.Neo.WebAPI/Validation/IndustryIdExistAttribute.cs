using SE.Neo.Core.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Validation
{
    public class IndustryIdExistAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var id = (int)value;
            if (id <= 0)
            {
                return new ValidationResult($"{validationContext.DisplayName} must be a valid industry id.");
            }

            ICompanyService service = (ICompanyService)validationContext.GetService(typeof(ICompanyService));

            var exist = service.IsIndustryIdExistAsync(id).Result;
            return exist ? ValidationResult.Success : new ValidationResult($"{validationContext.DisplayName} must be a valid industry id.");
        }
    }
}
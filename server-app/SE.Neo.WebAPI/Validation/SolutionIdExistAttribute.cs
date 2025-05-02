using SE.Neo.Core.Services.Interfaces;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Validation
{
    public class SolutionIdExistAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var id = (value as int?) ?? 0;

            ICommonService service = (ICommonService)validationContext.GetService(typeof(ICommonService));

            var exist = service.IsSolutionIdExistAsync(id).Result;
            return exist ? ValidationResult.Success : new ValidationResult($"{validationContext.DisplayName} must be a valid solution id.");
        }
    }
}
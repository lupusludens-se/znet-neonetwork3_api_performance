using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace SE.Neo.WebAPI.Extensions
{
    public static class ValidationResultExtensions
    {
        public static void AddToModelState(this ValidationResult result, ModelStateDictionary modelState)
        {
            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    modelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }
            }
        }
    }
}

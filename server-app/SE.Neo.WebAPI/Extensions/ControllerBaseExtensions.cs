using FluentValidation;
using FluentValidation.Results;
using Microsoft.AspNetCore.Mvc;
using SE.Neo.WebAPI.Models.Shared;

namespace SE.Neo.WebAPI.Extensions
{
    public static class ControllerBaseExtensions
    {
        public static async Task<UnprocessableEntityObjectResult?> ManualValidation<T>(this ControllerBase controller, T model, IValidator<T> validator)
        {
            ValidationResult validationResult = await validator.ValidateAsync(model);
            if (!validationResult.IsValid)
            {
                validationResult.AddToModelState(controller.ModelState);
                return controller.UnprocessableEntity(new ValidationResponse(controller.ModelState));
            }

            return null;
        }
    }
}

using Microsoft.AspNetCore.Mvc.ModelBinding;
using SE.Neo.Common.Extensions;

namespace SE.Neo.WebAPI.Models.Shared
{
    public class ValidationResponse
    {
        public Dictionary<string, List<string?>> Errors { get; set; }

        public ValidationResponse(ModelStateDictionary modelState)
        {
            if (modelState.TryGetValue(string.Empty, out var modelStateEntry))
            {
                modelState.Remove(string.Empty);
            }

            Errors = modelState
                .Where(x => x.Value?.Errors.Count > 0)
                .ToDictionary(
                    kvp => kvp.Key.ToLower(0, 1),
                    kvp => kvp.Value!.Errors
                        .Select(er => !string.IsNullOrEmpty(er.ErrorMessage)
                            ? er.ErrorMessage
                            : er.Exception?.Message)
                        .ToList());
        }
    }
}

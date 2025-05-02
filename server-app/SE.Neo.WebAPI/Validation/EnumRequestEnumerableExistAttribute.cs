using SE.Neo.WebAPI.Models.Shared;
using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Validation
{
    public class EnumRequestEnumerableExistAttribute : ValidationAttribute
    {
        private readonly Type _enumType;
        private readonly string _errorMessage;

        public EnumRequestEnumerableExistAttribute(Type enumType, string errorMessage = null)
        {
            _enumType = enumType;
            _errorMessage = errorMessage;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            var enumRequests = (IEnumerable)typeof(Enumerable)
                .GetMethod("Cast")
                .MakeGenericMethod(typeof(EnumRequest<>).MakeGenericType(_enumType))
                .Invoke(null, new object[] { (IEnumerable)value });

            foreach (dynamic request in enumRequests)
                if (!Enum.IsDefined(_enumType, request.Id))
                    return new ValidationResult($"{validationContext.DisplayName} — {_errorMessage}.");

            return ValidationResult.Success;
        }
    }
}

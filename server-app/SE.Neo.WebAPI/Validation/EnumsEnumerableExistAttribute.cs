using System.Collections;
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Validation
{
    public class EnumsEnumerableExistAttribute : ValidationAttribute
    {
        private readonly Type _enumType;
        private readonly string _errorMessage;

        public EnumsEnumerableExistAttribute(Type enumType, string errorMessage = null)
        {
            _enumType = enumType;
            _errorMessage = errorMessage;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
                return ValidationResult.Success;

            var ids = (IEnumerable)value;
            return ids.Cast<Enum>().All(id => Enum.IsDefined(_enumType, id)) ?
                ValidationResult.Success :
                    new ValidationResult($"{validationContext.DisplayName} — {_errorMessage}.");
        }
    }
}

using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Validation
{
    public class EnumExistAttribute : ValidationAttribute
    {
        private readonly Type _enumType;
        private readonly string _errorMessage;

        public EnumExistAttribute(Type enumType, string errorMessage = null)
        {
            _enumType = enumType;
            _errorMessage = errorMessage;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var id = value as Enum;
            if (id == null)
                return ValidationResult.Success;

            return _enumType.IsEnum && Enum.IsDefined(_enumType, id) ?
                ValidationResult.Success :
                    new ValidationResult($"{validationContext.DisplayName} — {_errorMessage}.");
        }
    }
}

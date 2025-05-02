using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Validation
{
    public class RequiredIfAttribute : ValidationAttribute
    {
        private readonly string _dependentOnProperty;
        private readonly object _value;

        public RequiredIfAttribute(string dependentOnProperty, object value)
        {
            _dependentOnProperty = dependentOnProperty;
            _value = value;

        }

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var dependentOnProperty = validationContext.ObjectType.GetProperty(_dependentOnProperty);
            if (dependentOnProperty == null)
                return new ValidationResult($"Unknown property {_dependentOnProperty}.");

            var dependentOnPropertyValue = dependentOnProperty.GetValue(validationContext.ObjectInstance, null);
            if (_value != null ? _value.Equals(dependentOnPropertyValue) : _value == dependentOnPropertyValue)
                return value == null
                    ? new ValidationResult($"{validationContext.MemberName} is required if {_dependentOnProperty} is {_value}.")
                    : ValidationResult.Success;
            else
                return ValidationResult.Success;
        }
    }
}
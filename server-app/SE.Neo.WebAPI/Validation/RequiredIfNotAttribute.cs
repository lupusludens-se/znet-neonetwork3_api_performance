using System.ComponentModel.DataAnnotations;

namespace SE.Neo.WebAPI.Validation
{
    public class RequiredIfNotAttribute : ValidationAttribute
    {
        private readonly string _dependentOnProperty;
        private readonly object[] _values;

        public RequiredIfNotAttribute(string dependentOnProperty, object value)
        {
            _dependentOnProperty = dependentOnProperty;
            _values = new[] { value };
        }

        public RequiredIfNotAttribute(string dependentOnProperty, object[] values)
        {
            _dependentOnProperty = dependentOnProperty;
            _values = values;
        }

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var dependentOnProperty = validationContext.ObjectType.GetProperty(_dependentOnProperty);
            if (dependentOnProperty == null)
                return new ValidationResult($"Unknown property {_dependentOnProperty}.");

            var dependentOnPropertyValue = dependentOnProperty.GetValue(validationContext.ObjectInstance, null);
            if (!_values.Any(v => v != null ? v.Equals(dependentOnPropertyValue) : v == dependentOnPropertyValue))
                return value == null
                    ? new ValidationResult($"{validationContext.MemberName} is required if {_dependentOnProperty} is not {string.Join(", ", _values)}.")
                    : ValidationResult.Success;
            else
                return ValidationResult.Success;
        }
    }
}
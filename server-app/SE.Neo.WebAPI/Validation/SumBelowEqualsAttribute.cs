using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace SE.Neo.WebAPI.Validation
{
    public class SumBelowEqualsAttribute : ValidationAttribute
    {
        private readonly int _max;
        private readonly string[] _propertiesNames;

        public SumBelowEqualsAttribute(int max, params string[] propertiesNames)
        {
            _max = max;
            _propertiesNames = propertiesNames;
        }

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            int? sum = (int?)value;
            foreach (string propertyName in _propertiesNames)
            {
                PropertyInfo? propertyInfo = validationContext.ObjectType.GetProperty(propertyName);
                if (propertyInfo == null)
                    return new ValidationResult($"Unknown property {propertyName}.");

                int? propertyValue = (int?)propertyInfo.GetValue(validationContext.ObjectInstance, null);
                sum += propertyValue;
            }

            return sum == null || sum <= _max ? ValidationResult.Success : new ValidationResult($"Sum with {string.Join(", ", _propertiesNames)} must be below or equal {_max}.");
        }
    }
}
using System.ComponentModel.DataAnnotations;

namespace SE.Neo.Common.Attributes
{
    public class OrderWhiteListAttribute : ValidationAttribute
    {
        private readonly string[] _whiteList;

        public OrderWhiteListAttribute(string[] whiteList)
        {
            _whiteList = whiteList.Select(str => str.ToLower()).ToArray();
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return ValidationResult.Success!;
            }
            var filter = value as string;
            if (!_whiteList.Contains(filter.ToLower()))
            {
                return new ValidationResult($"{filter} is not allowed.");
            }

            return ValidationResult.Success;
        }
    }
}
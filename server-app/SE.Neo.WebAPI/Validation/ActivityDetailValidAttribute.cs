using SE.Neo.Core.Enums;
using SE.Neo.Core.Services.Interfaces;
using SE.Neo.WebAPI.Constants;
using System.ComponentModel.DataAnnotations;
using System.Reflection;

namespace SE.Neo.WebAPI.Validation
{
    public class ActivityDetailValidAttribute : ValidationAttribute
    {
        public ActivityDetailValidAttribute(string activityTypeProperty, string activityLocationProperty)
        {
            if (activityTypeProperty == null)
            {
                throw new ArgumentNullException("activityTypeField");
            }
            if (activityLocationProperty == null)
            {
                throw new ArgumentNullException("activityLocationField");
            }
            ActivityTypeProperty = activityTypeProperty;
            ActivityLocationProperty = activityLocationProperty;
        }

        public string ActivityTypeProperty { get; private set; }
        public string ActivityLocationProperty { get; private set; }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (!TryGetPropertyValue(validationContext, ActivityTypeProperty, out int type) ||
               !TryGetPropertyValue(validationContext, ActivityLocationProperty, out int location))
            {
                return new ValidationResult(ErrorMessages.ModelStateInvalid);
            }

            IActivityService activityService = (IActivityService)validationContext.GetService(typeof(IActivityService))!;

            bool isValid = activityService.IsValid(value.ToString()!, (ActivityType)type, (ActivityLocation)location);
            return isValid ? ValidationResult.Success : new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
        }

        private bool TryGetPropertyValue(ValidationContext validationContext, string property, out int propertyValue)
        {
            PropertyInfo? propertyInfo = validationContext.ObjectType.GetProperty(property);
            if (propertyInfo == null)
            {
                propertyValue = 0;
                return false;
            }

            return int.TryParse(((int)propertyInfo.GetValue(validationContext.ObjectInstance)!).ToString(), out propertyValue);
        }
    }
}

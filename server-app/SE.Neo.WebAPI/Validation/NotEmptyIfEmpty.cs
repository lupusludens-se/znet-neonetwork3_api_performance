using System.ComponentModel.DataAnnotations;
namespace SE.Neo.WebAPI.Validation
{
    ///<summary>
    ///This attribute creates an exclusive or required relationship between 2 lists of int
    ///Validation success if one list and not the other is supplied
    ///</summary>
    public class NotEmptyIfEmpty : ValidationAttribute
    {
        private readonly string _dependentOnProperty;
        public NotEmptyIfEmpty(string dependentOnProperty)
        {
            _dependentOnProperty = dependentOnProperty;
        }

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            bool isListNullOrEmpty = true;
            if (value is IEnumerable<int> list && list.Any())
            {
                isListNullOrEmpty = false;
            }
            object? dependentOnPropertyValue = validationContext.ObjectType.GetProperty(_dependentOnProperty)?.GetValue(validationContext.ObjectInstance, null);
            bool isDependentNullOrEmpty = true;
            if (dependentOnPropertyValue is IEnumerable<int> dependentList && dependentList.Any())
            {
                isDependentNullOrEmpty = false;
            }

            if (isListNullOrEmpty ^ isDependentNullOrEmpty)
            {
                return ValidationResult.Success;
            }
            else
            {
                return new ValidationResult($"Must supply {validationContext.MemberName} or {_dependentOnProperty} but not both.");
            }
        }
    }
}

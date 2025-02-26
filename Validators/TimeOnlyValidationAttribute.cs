using System.ComponentModel.DataAnnotations;

namespace projektas.Validators
{
    public class TimeOnlyValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is TimeOnly)
            {
                return ValidationResult.Success;
            }
            return new ValidationResult(ErrorMessage ?? "Invalid time format.");
        }
    }
}
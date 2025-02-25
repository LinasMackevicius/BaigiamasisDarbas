using System.ComponentModel.DataAnnotations;

namespace projektas.Validators
{
    public class TimeOnlyValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is TimeOnly timeValue)
            {
                // Check if it's a valid TimeOnly value (this is usually implicit)
                return ValidationResult.Success; // Always valid for any TimeOnly value
            }
            return new ValidationResult(ErrorMessage ?? "Invalid time format.");
        }
    }
}
using Common.Constants;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Common.Validation
{
    public sealed class EmailFormatAttribute : ValidationAttribute
    {
        public new string ErrorMessage { get; set; } = string.Empty;

        public EmailFormatAttribute() { }

        public EmailFormatAttribute(string errorMessage) : this() => ErrorMessage = errorMessage;

        override protected ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var emailAddress = !string.IsNullOrWhiteSpace(value as string) ? value.ToString() : string.Empty;

            if (!Patterns.EMAIL_PATTERN.IsMatch(emailAddress ?? string.Empty)) return new ValidationResult(ErrorMessage);
            return ValidationResult.Success ?? default!;
        }
    }
}

using Common.Constants;
using Common.Exceptions;

namespace Common.Extensions
{
    public static class EmailValidationExtension
    {
        public static string ValidateEmail(this string email)
        {
            email = email.Trim();

            CustomValidationException.Validate(string.IsNullOrEmpty(email), AppConstants.REQUIRED_EMAIL_ERROR);
            CustomValidationException.Validate(!string.IsNullOrEmpty(email) && !Patterns.EMAIL_PATTERN.IsMatch(email), AppConstants.INVALID_EMAIL_ERROR, null);

            return email;
        }
    }
}

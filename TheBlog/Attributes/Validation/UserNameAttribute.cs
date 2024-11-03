using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace TheBlog.Attributes.Validation
{
    public class UserNameAttribute : ValidationAttribute
    {
        private readonly string _pattern = @"^[a-zA-Z0-9\-\._\@\+]+$";
        private readonly int _minLength = 5;
        private readonly int _maxLength = 30;

        public string GetMatchError() =>
            "Only letters, digits and characters \"-._@+\" are allowed.";

        public string GetLengthError() =>
            $"User name must be between {_minLength} and {_maxLength} characters long.";

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var username = value as string;

            if (username == null)
            {
                return ValidationResult.Success;
            }

            if (!(_minLength <= username.Length && username.Length <= _maxLength))
            {
                return new ValidationResult(GetLengthError());
            }

            if (!Regex.IsMatch(username, _pattern))
            {
                return new ValidationResult(GetMatchError());
            }

            return ValidationResult.Success;
        }
    }
}

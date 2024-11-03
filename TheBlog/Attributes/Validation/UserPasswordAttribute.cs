using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace TheBlog.Attributes.Validation
{
    public class UserPasswordAttribute : ValidationAttribute
    {
        private readonly string _pattern = @"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).+$";
        private readonly int _minLength = 7;
        private readonly int _maxLength = 200;

        public string GetMatchError() =>
            "At least one each of these has to be in the password: upper and lower case english letters, digit and special character.";

        public string GetLengthError() =>
            $"Password must be between {_minLength} and {_maxLength} characters long.";

        protected override ValidationResult IsValid(object? value, ValidationContext validationContext)
        {
            var password = value as string;

            if (password == null)
            {
                return ValidationResult.Success;
            }

            if (!(_minLength <= password.Length && password.Length <= _maxLength))
            {
                return new ValidationResult(GetLengthError());
            }

            if (!Regex.IsMatch(password, _pattern))
            {
                return new ValidationResult(GetMatchError());
            }

            return ValidationResult.Success;
        }
    }
}

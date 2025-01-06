using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;

namespace TimeManagementApp.Helper
{
    /// <summary>
    /// This class is used to check the format of the input data.
    /// </summary>
    public class CheckingFormatHelper
    {
        const int MAX_LENGTH = 50;
        const int MIN_LENGTH = 8;
        public static (bool ok, string error) CheckUsernameFormat(string username)
        {
            if (username.IsNullOrEmpty())
            {
                return (false, $"Please_fill_out_your_username".GetLocalized());
            }

            if (username.Length >= MAX_LENGTH) { 
                return (false, $"Username_must_have_characters_long_less_than".GetLocalized() + $" {MAX_LENGTH}");
            }
            return (true, null);
        }
        public static (bool ok, string error) CheckPasswordFormat(string password)
        {
            if (password.Length < MIN_LENGTH)
            {
                return (false, $"Pasword_must_have_characters_long_more_than".GetLocalized() + $" {MIN_LENGTH}");
            }

            if (password.Length >= MAX_LENGTH)
            {
                return (false, $"Pasword_must_have_characters_long_less_than".GetLocalized() + $" {MAX_LENGTH}");
            }

            // Regular expression pattern for validating password
            var regex = new Regex(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d).{8,}$");

            if (!regex.IsMatch(password))
            {
                if (!Regex.IsMatch(password, @"[A-Z]"))
                    return (false, "Password_must_contain_at_least_one_uppercase_letter".GetLocalized());

                if (!Regex.IsMatch(password, @"[a-z]"))
                    return (false, "Password_must_contain_at_least_one_lowercase_letter".GetLocalized());

                if (!Regex.IsMatch(password, @"\d"))
                    return (false, "Password_must_contain_at_least_one_digit".GetLocalized());
            }
            return (true, null);
        }
        public static (bool ok, string error) CheckEmailFormat(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return (false, "Please_fill_out_your_email".GetLocalized());
            }
            // Regular expression pattern for validating email
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            Regex regex = new Regex(pattern);
            if (!regex.IsMatch(email))
            {
                return (false, "Please_fill_out_a_valid_email_address_format".GetLocalized());
            }
            return (true, null);
        }
        public static (bool ok, string error) CheckPasswordConfirmed(string password, string passwordConfirmed)
        {
            if (password != passwordConfirmed)
            {
                return (false, "Password_confirmation_does_not_match_the_password".GetLocalized());
            }
            return (true, null);
        }
        public static (bool ok, string error) CheckAll(string username, string email, string password, string passwordConfirmed)
        {
            (bool ok, string error) = CheckUsernameFormat(username);
            if (!ok) return (ok, error);
            (ok, error) = CheckEmailFormat(email);
            if (!ok) return (ok, error);
            (ok, error) = CheckPasswordFormat(password);
            if (!ok) return (ok, error);
            (ok, error) = CheckPasswordConfirmed(password, passwordConfirmed);
            if (!ok) return (ok, error);
            return (true, null);
        }
        public static (bool ok, string error) CheckAll(string password, string passwordConfirmed)
        {
            (bool ok, string error) = CheckPasswordFormat(password);
            if (!ok) return (ok, error);
            (ok, error) = CheckPasswordConfirmed(password, passwordConfirmed);
            if (!ok) return (ok, error);
            return (true, null);
        }
    }
}

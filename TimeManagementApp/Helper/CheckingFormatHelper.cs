using Microsoft.IdentityModel.Tokens;
using System.Text.RegularExpressions;

namespace TimeManagementApp.Helper
{
    public class CheckingFormatHelper
    {
        const int MAX_LENGTH = 50;
        const int MIN_LENGTH = 8;
        public static (bool ok, string error) CheckUsernameFormat(string username)
        {
            if (username.IsNullOrEmpty())
            {
                return (false, $"Username can not be empty");
            }

            if (username.Length > MAX_LENGTH) { 
                return (false, $"Username must be less than {MAX_LENGTH} characters long");
            }
            return (true, null);
        }
        public static (bool ok, string error) CheckPasswordFormat(string password)
        {
            if (password.Length < MIN_LENGTH)
            {
                return (false, $"Password must be at least {MIN_LENGTH} characters long");
            }

            if (password.Length > MAX_LENGTH)
            {
                return (false, $"Password must be less than {MAX_LENGTH} characters long");
            }

            // Regular expression pattern for validating password
            var regex = new Regex(@"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d).{8,}$");

            if (!regex.IsMatch(password))
            {
                if (!Regex.IsMatch(password, @"[A-Z]"))
                    return (false, "Password must contain at least one uppercase letter");

                if (!Regex.IsMatch(password, @"[a-z]"))
                    return (false, "Password must contain at least one lowercase letter");

                if (!Regex.IsMatch(password, @"\d"))
                    return (false, "Password must contain at least one digit");
            }
            return (true, null);
        }
        public static (bool ok, string error) CheckEmailFormat(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return (false, "Please fill out your email.");
            }
            // Regular expression pattern for validating email
            string pattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
            Regex regex = new Regex(pattern);
            if (!regex.IsMatch(email))
            {
                return (false, "Please fill out a valid email address format.");
            }
            return (true, null);
        }
        public static (bool ok, string error) CheckPasswordConfirmed(string password, string passwordConfirmed)
        {
            if (password != passwordConfirmed)
            {
                return (false, "Password confirmation doesn't match the password");
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

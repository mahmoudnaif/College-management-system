using System.Text.RegularExpressions;

namespace College_managemnt_system.Repos.Utilities
{
    public class UtilitiesRepo
    {
        public bool IsValidEmail(string email)
        {
            string emailRegex = @"^[a-zA-Z0-9_.+-]+@[a-zA-Z0-9-]+\.[a-zA-Z0-9-.]+$";

            return Regex.IsMatch(email, emailRegex);
        }
        public bool IsValidPassword(string password)
        {
            string passwordRegex = @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)[a-zA-Z\d]{8,}$";

            return Regex.IsMatch(password, passwordRegex);
        }

        public bool IsValidUsername(string username)
        {
            string usernameRegex = @"^[a-zA-Z0-9]([_](?![_])|[a-zA-Z0-9]){3,16}[a-zA-Z0-9]$";

            return Regex.IsMatch(username, usernameRegex);
        }

    }
}

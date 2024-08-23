using System.Text.RegularExpressions;

namespace College_managemnt_system.Repos.Utilities
{
    public class UtilitiesRepo
    {
        public bool IsValidNationalId(string nationalId)
        {
            string nationalIdlRegex = @"^([2-3]{1})([0-9]{2})(0[1-9]|1[012])(0[1-9]|[1-2][0-9]|3[0-1])(0[1-4]|[1-2][1-9]|3[1-5]|88)[0-9]{3}([0-9]{1})[0-9]{1}$";

            return Regex.IsMatch(nationalId, nationalIdlRegex);
        }  
        public bool IsValidPhoneNumber(string phone)
        {
            string phoneRegex = @"^(\+201|01|00201)[0-2,5]{1}[0-9]{8}";

            return Regex.IsMatch(phone, phoneRegex);
        }
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

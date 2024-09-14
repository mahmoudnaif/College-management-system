using College_managemnt_system.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace College_managemnt_system.ClientModels
{
    public class StudentsInputModelCSV
    {
       
        public string FirstName { get; set; } = string.Empty;
        public string FathertName { get; set; } = string.Empty;    
        public string GrandfatherName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string NationalNumber { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Role { get; } = "student";
        public string PasswordAsString { get; } = GenerateRandomPassword();
        public byte[] Password { get; set; } = null!;


        // public DateTime? EnrollmentDate { get; set; }

        public static string GenerateRandomPassword(int length = 12)
        {
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()";
            StringBuilder password = new StringBuilder();
            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] uintBuffer = new byte[sizeof(uint)];

                while (length-- > 0)
                {
                    rng.GetBytes(uintBuffer);
                    uint num = BitConverter.ToUInt32(uintBuffer, 0);
                    password.Append(validChars[(int)(num % (uint)validChars.Length)]);
                }
            }
            return password.ToString();

        }
    }

 
}



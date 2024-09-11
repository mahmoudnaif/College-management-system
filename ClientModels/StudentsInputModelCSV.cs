using College_managemnt_system.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace College_managemnt_system.ClientModels
{
    public class StudentsInputModelCSV
    {
       
        public string FirstName { get; set; } = null!;
        public string FathertName { get; set; } = null!;
        public string GrandfatherName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string NationalNumber { get; set; } = null!;
        public string Email { get; set; } = null!;
        public string Role { get; } = "student";
        public string PasswordAsString { get; } = GenerateRandomPassword();
        public byte[] Password { get; set; } = null!;


        // public DateTime? EnrollmentDate { get; set; }

        private static string GenerateRandomPassword(int length = 12)
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

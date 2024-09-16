namespace College_managemnt_system.ClientModels
{
    public class TeachingAssistanceInputModelCSV
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string NationalNumber { get; set; } = null!;
        public DateTime HiringDate { get; set; } = DateTime.Now;
        public string email { get; set; } = null!;
        public string Role { get; } = "ta";
        public string PasswordAsString { get; } = StudentsInputModelCSV.GenerateRandomPassword(); //the function should exist in onether class
        public byte[] Password { get; set; } = null!;
    }
}

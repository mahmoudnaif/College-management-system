namespace College_managemnt_system.ClientModels
{
    public class ProfessorInputModelCSV
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string NationalNumber { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public DateTime HiringDate { get; set; } = DateTime.Now;
        public string DepartmentName { get; set; } = string.Empty;
        public int DepartmentId { get; set; }
        public string email { get; set; } = string.Empty;
        public string Role { get; } = "prof";
        public string PasswordAsString { get; } = StudentsInputModelCSV.GenerateRandomPassword(); //the function should exist in onether class
        public byte[] Password { get; set; } = null!;
    }
}

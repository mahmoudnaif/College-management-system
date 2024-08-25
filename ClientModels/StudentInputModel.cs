namespace College_managemnt_system.ClientModels
{
    public class StudentInputModel
    {
        public string FirstName { get; set; } = null!;
        public string FathertName { get; set; } = null!;
        public string GrandfatherName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string NationalNumber { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime? EnrollmentDate { get; set; }
    }
}

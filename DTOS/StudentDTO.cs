namespace College_managemnt_system.DTOS
{
    public class StudentDTO
    {
        public int StudentId { get; set; }
        public string FirstName { get; set; } = null!;
        public string FathertName { get; set; } = null!;
        public string GrandfatherName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public string NationalNumber { get; set; } = null!;
        public float Cgpa { get; set; }
        public DateTime EnrollmentDate { get; set; }
        public int TotalHours { get; set; }
        public int? DepartmentId { get; set; }
    }
}
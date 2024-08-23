namespace College_managemnt_system.ClientModels
{
    public class ProfessorInputModel
    {
        public string FirstName { get; set; } = null!;
        public string LastName { get; set; } = null!;
        public string NationalNumber { get; set; } = null!;
        public string Phone { get; set; } = null!;
        public DateTime HiringDate { get; set; }
        public int DepartmentId { get; set; }
        public string email { get; set; } = null!;
    }

    
}

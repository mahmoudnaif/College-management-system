namespace College_managemnt_system.DTOS
{
    public class StudentCoursesDTO
    {
        public int courseId { get; set; }
        public string courseName { get; set; } = null!;
        public string professorName { get; set; } = null!;
        public string courseCode { get; set; } = null!;
        public int credits {  get; set; }
        public string status { get; set; } = null!;

        public string? grade { get; set; } 


    }
}

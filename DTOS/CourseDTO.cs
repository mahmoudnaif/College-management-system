namespace College_managemnt_system.DTOS
{
    public class CourseDTO
    {
        public int CourseId { get; set; }

        public string CourseName { get; set; } = null!;

        public string CourseCode { get; set; } = null!;

        public int Credits { get; set; }

        public int DepartmentId { get; set; }
    }
}

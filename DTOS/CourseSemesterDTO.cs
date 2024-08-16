namespace College_managemnt_system.DTOS
{
    public class CourseSemesterDTO
    {
        public int CourseSemesterId { get; set; }

        public int ProfessorId { get; set; }

        public int CourseId { get; set; }

        public int SemesterId { get; set; }

        public bool Isactive { get; set; }

        public string CourseName { get; set; } = null!;
    }
}

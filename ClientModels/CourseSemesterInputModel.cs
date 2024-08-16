namespace College_managemnt_system.ClientModels
{
    public class CourseSemesterInputModel
    {
        public int ProfessorId { get; set; }

        public int CourseId { get; set; }

        public int SemesterId { get; set; }

        public bool Isactive { get; set; } = false;
    }

    public class ChangeProfInputModel
    {
        public int CourseSemesterId { get; set; }
        public int ProfessorId { get; set; }
    }

    public class EditActivationStatus
    {
        public int CourseSemesterId { get; set; }
        public bool Isactive { get; set; }
    }
}

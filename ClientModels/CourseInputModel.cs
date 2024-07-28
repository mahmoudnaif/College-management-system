namespace College_managemnt_system.ClientModels
{
    public class CourseInputModel
    {
        public string CourseName { get; set; } = null!;

        public string CourseCode { get; set; } = null!;

        public int Credits { get; set; }

        public int DepartmentId { get; set; }
    }


    public class CourseEditModel
    {
        public int CourseId { get; set; }
        public string Editproberty { get; set; }
    }
}

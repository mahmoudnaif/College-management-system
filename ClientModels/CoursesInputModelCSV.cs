namespace College_managemnt_system.ClientModels
{
    public class CoursesInputModelCSV
    {
        public string CourseName { get; set; } = string.Empty;
        public string CourseCode { get; set; } = string.Empty;
        public int Credits { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public List<string> PrereqsCoursesCodes { get; set; } = []; //collection initializers instead of new list<string>();;
        public int DepartmentId { get; set; }
    }
}

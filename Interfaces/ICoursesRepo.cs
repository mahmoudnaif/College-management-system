using College_managemnt_system.ClientModels;
using College_managemnt_system.CustomResponse;
using College_managemnt_system.DTOS;
namespace College_managemnt_system.Interfaces
{
    public interface ICoursesRepo
    {
        public Task<CustomResponse<IEnumerable<CourseDTO>>> GetCourses(TakeSkipModel takeSkipModel);
        public Task<CustomResponse<CourseDTO>> GetCourseByCourseCode(string courseCode);
        public Task<CustomResponse<CourseDTO>> AddCourse(CourseInputModel courseInputModel);
        public Task<CustomResponse<bool>> DeleteCourse(int courseId);
        public Task<CustomResponse<CourseDTO>> EditCreditHours(int courseId, int credits);
        public Task<CustomResponse<CourseDTO>> EditCourseCode(int courseId, string courseCode);
        public Task<CustomResponse<CourseDTO>> EditCourseName(int courseId,string newName);
        public Task<CustomResponse<CourseDTO>> EditDepartment(int courseId, int DepartmentId);
    }
}

using College_managemnt_system.ClientModels;
using College_managemnt_system.CustomResponse;
using College_managemnt_system.DTOS;

namespace College_managemnt_system.Interfaces
{
    public interface IStudentCoursesRepo
    {
        public Task<CustomResponse<List<StudentCoursesDTO>>> GetActiveStudentCourses(int studentId);
        public Task<CustomResponse<List<StudentCoursesDTO>>> GetAllFinishedStudentCourses(int studentId,TakeSkipModel takeSkipModel);
        public Task<CustomResponse<bool>> DropCourse(int studentId,int courseId);
        public Task<CustomResponse<bool>> WithDrawlFromCourse(int studentId,int courseId,bool hasReason);
        public Task<CustomResponse<bool>> SetCourseIncomplete(int studentId,int courseId);
        public Task<CustomResponse<bool>> SetGrade(int studentId,int courseId,string grade);
        public Task<CustomResponse<bool>> AppealGrade(int studentId,int courseId, string grade);
    }
}

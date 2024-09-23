using College_managemnt_system.ClientModels;
using College_managemnt_system.CustomResponse;
using College_managemnt_system.DTOS;
using Org.BouncyCastle.X509.Extension;

namespace College_managemnt_system.Interfaces
{
    public interface IRegisterSemesterCoursesRepo //TODO: Finish student registration.
    {
        public Task<CustomResponse<List<CourseDTO>>> GetAvailableCourses(int studentId);
        public Task<CustomResponse<object>> GetAvailableSchedule(List<int> courseIds);
        public Task<CustomResponse<bool>> RegisterCourses_SchedulesByGroup(int studentId,List<int> courseIds,int groupId,bool bypassrules = false);
        public Task<CustomResponse<object>> RegisterCustomCourses_Schedules(int studentId,List<CustomGroupCourseInputModel> courseIds, bool bypassrules = false);
    }
}

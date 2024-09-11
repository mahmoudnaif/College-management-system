using College_managemnt_system.CustomResponse;
using College_managemnt_system.DTOS;
using Org.BouncyCastle.X509.Extension;

namespace College_managemnt_system.Interfaces
{
    public interface IRegisterCourses //TODO: Finish student registration.
    {
        public Task<CustomResponse<bool>> GetAvailableCourses(int studentId);
        public Task<bool> CoursesExistInOneGroup(List<int> courseSemesterIds);
        public Task<CustomResponse<bool>> GetAvailableScheduleByGroup(List<int> courseSemesterIds);
        public Task<CustomResponse<bool>> GetAvailableCustomSchedule(List<int> courseSemesterIds);
        public Task<CustomResponse<bool>> RegisterCourses_Schedules();
    }
}

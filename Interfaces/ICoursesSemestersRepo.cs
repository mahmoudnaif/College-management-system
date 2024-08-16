using College_managemnt_system.DTOS;
using College_managemnt_system.CustomResponse;
using College_managemnt_system.ClientModels;

namespace College_managemnt_system.Interfaces
{
    public interface ICoursesSemestersRepo
    {
        public Task<CustomResponse<IEnumerable<CourseSemesterDTO>>> GetCoursesBySemester(int semesterId);
        public Task<CustomResponse<IEnumerable<CourseSemesterDTO>>> GetActiveSemesterCourses();
        public Task<CustomResponse<bool>> EditActiveStatusForAllCourses(bool isActive); // activates or deactivates all the semestercourses assosiated to the current active semester
        public Task<CustomResponse<CourseSemesterDTO>> Add(CourseSemesterInputModel model);

        public Task<CustomResponse<bool>> Delete(int courseSemesterId);

        public Task<CustomResponse<CourseSemesterDTO>> ChangeProfessor(ChangeProfInputModel model);

        public Task<CustomResponse<CourseSemesterDTO>> EditActivationStatus(EditActivationStatus editActivationStatus);
    }
}

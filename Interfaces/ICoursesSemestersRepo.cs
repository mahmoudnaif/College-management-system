using College_managemnt_system.DTOS;
using College_managemnt_system.CustomResponse;
using College_managemnt_system.ClientModels;
using Microsoft.AspNetCore.Mvc;

namespace College_managemnt_system.Interfaces
{
    public interface ICoursesSemestersRepo
    {
        public Task<CustomResponse<List<CourseSemesterDTO>>> GetCoursesBySemester(int semesterId);
        public Task<CustomResponse<List<CourseSemesterDTO>>> GetActiveSemesterCourses();
        public Task<CustomResponse<bool>> EditActiveStatusForAllCourses(bool isActive); // activates or deactivates all the semestercourses assosiated to the current active semester
        public Task<CustomResponse<CourseSemesterDTO>> Add(CourseSemesterInputModel model);

        public Task<CustomResponse<bool>> Delete(int courseId, int semesterId);

        public Task<CustomResponse<CourseSemesterDTO>> ChangeProfessor(int courseId, int semesterId, int profId);

        public Task<CustomResponse<CourseSemesterDTO>> EditActivationStatus(int courseId, int semesterId, bool isActive);
    }
}

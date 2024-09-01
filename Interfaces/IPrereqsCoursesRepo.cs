using College_managemnt_system.ClientModels;
using College_managemnt_system.CustomResponse;
using College_managemnt_system.DTOS;
using Microsoft.AspNetCore.Mvc;
namespace College_managemnt_system.Interfaces
{
    public interface IPrereqsCoursesRepo
    {
        public Task<CustomResponse<List<CourseDTO>>> GetPrereqsCourses(int courseId);

        public Task<CustomResponse<PrereqDTO>> AddPrereqsCourse(int courseId,  int prereqsCourseId);

        public Task<CustomResponse<PrereqDTO>> AddPrereqsCourse(int courseId, string prereqsCourseCode);

        public Task<CustomResponse<bool>> RemovePrereqsCourse(int courseId,int prereqsCourseId);
    }
}

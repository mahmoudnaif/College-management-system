using College_managemnt_system.ClientModels;
using College_managemnt_system.CustomResponse;
using College_managemnt_system.DTOS;
namespace College_managemnt_system.Interfaces
{
    public interface IPrereqsCoursesRepo
    {
        public Task<CustomResponse<List<CourseDTO>>> GetPrereqsCourses(int courseId);

        public Task<CustomResponse<PrereqDTO>> AddPrereqsCourse(PrereqsInputModel prereqsInputModel);

        public Task<CustomResponse<bool>> RemovePrereqsCourse(PrereqsInputModel prereqsInputModel);
    }
}

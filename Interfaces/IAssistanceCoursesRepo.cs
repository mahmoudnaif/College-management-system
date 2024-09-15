using College_managemnt_system.CustomResponse;
using College_managemnt_system.DTOS;

namespace College_managemnt_system.Interfaces
{
    public interface IAssistanceCoursesRepo
    {
        public Task<CustomResponse<AssistantsCoursesDTO>> AddTaToCourse(int courseId, int semesterId, int taId);

        public Task<CustomResponse<bool>> RemoveTaFromCourse(int courseId, int semesterId, int taId);

         public Task<CustomResponse<List<TeachingAssistanceDTO>>> GetCourseTas(int courseId, int semesterId);

        public Task<CustomResponse<List<CourseSemesterDTO>>> getTaCourses(int semesterId, int taId);
    }
}

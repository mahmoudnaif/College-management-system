using College_managemnt_system.CustomResponse;
using College_managemnt_system.DTOS;

namespace College_managemnt_system.Interfaces
{
    public interface IAssistanceCoursesRepo
    {
        public Task<CustomResponse<AssistantsCoursesDTO>> AddTaToCourse(int courseSemesterId,int taId);

        public Task<CustomResponse<bool>> RemoveTaFromCourse(int courseSemesterId, int taId);

         public Task<CustomResponse<List<TeachingAssistanceDTO>>> GetCourseTas(int courseSemesterId);

        public Task<CustomResponse<List<CourseSemesterDTO>>> getTaCourses(int taId);
    }
}

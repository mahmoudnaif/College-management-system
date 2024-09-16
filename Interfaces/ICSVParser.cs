using College_managemnt_system.ClientModels;
using College_managemnt_system.CustomResponse;

namespace College_managemnt_system.Interfaces
{
    public interface ICSVParser
    {
        public Task<CustomResponse<List<StudentErrorSheet>>> AddStudents(IFormFile file);
        public Task<CustomResponse<List<ProfErrorSheet>>> AddProfessors(IFormFile file);
        public Task<CustomResponse<List<TeachingAssistantErrorSheet>>> AddTeachingAssistances(IFormFile file);
        public Task<CustomResponse<List<CourseErrorSheet>>> AddCourses(IFormFile file);
        public Task<CustomResponse<List<ClassRoomErrorSheet>>> AddClassRooms(IFormFile file);
    }
}
using College_managemnt_system.ClientModels;
using College_managemnt_system.CustomResponse;

namespace College_managemnt_system.Interfaces
{
    public interface ICSVParser
    {
        public Task<CustomResponse<List<StudentErrorSheet>>> AddStudents(IFormFile file);
        public Task<CustomResponse<bool>> AddProfessors(IFormFile file);
        public Task<CustomResponse<bool>> AddTeachingAssistances(IFormFile file);
        public Task<CustomResponse<bool>> AddCourses(IFormFile file);
    }
}
using College_managemnt_system.ClientModels;
using College_managemnt_system.CustomResponse;
using College_managemnt_system.DTOS;

namespace College_managemnt_system.Interfaces
{
    public interface IStudentRepo
    {
      
        public Task<CustomResponse<List<StudentDTO>>> GetStudentSByYear(int year,TakeSkipModel model);
        public Task<CustomResponse<StudentDTO>> GetStudentByEmail(string email);
        public Task<CustomResponse<StudentDTO>> GetStudentByNationalId(string nationalId);
        public Task<CustomResponse<List<StudentDTO>>> SearchStudents(string searchQuery,TakeSkipModel model);
        public Task<CustomResponse<StudentDTO>> Add(StudentInputModel model);
        public Task<CustomResponse<StudentDTO>> EditPhone(int studentId, string phoneNumber);
        public Task<CustomResponse<StudentDTO>> EditName(int studentId, FullNameInputModel model);
        public Task<CustomResponse<bool>> Delete(int studentId);

        public Task<CustomResponse<bool>> calculateALLStudentsCGPA_totalHours();
        public Task<CustomResponse<StudentDTO>> calculateStudentCGPA_totalHours(int sutdentId);


    }
}

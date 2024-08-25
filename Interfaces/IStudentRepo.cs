using College_managemnt_system.ClientModels;
using College_managemnt_system.CustomResponse;
using College_managemnt_system.DTOS;

namespace College_managemnt_system.Interfaces
{
    public interface IStudentRepo
    {
        public Task<CustomResponse<IEnumerable<StudentDTO>>> GetStudentSByYear(int year,TakeSkipModel model);
        public Task<CustomResponse<StudentDTO>> GetStudentByEmail(string email);
        public Task<CustomResponse<IEnumerable<StudentDTO>>> SearchStudentsByName(string searchQuery,TakeSkipModel model);
        public Task<CustomResponse<StudentDTO>> Add(StudentInputModel model);
        public Task<CustomResponse<StudentDTO>> EditPhone(int studentId, string phoneNumber);
        public Task<CustomResponse<StudentDTO>> EditName(int studentId, FullNameInputModel model);
        public Task<CustomResponse<bool>> Delete(int studentId);
    }
}

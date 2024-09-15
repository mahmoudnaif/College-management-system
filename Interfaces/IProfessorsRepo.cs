using College_managemnt_system.ClientModels;
using College_managemnt_system.CustomResponse;
using College_managemnt_system.DTOS;

namespace College_managemnt_system.Interfaces
{
    public interface IProfessorsRepo
    {
        public Task<CustomResponse<List<ProfessorDTO>>> SearchProfessors(string searchQuery,TakeSkipModel takeSkipModel);
        public Task<CustomResponse<ProfessorDTO>> GetProfByNationalId(string nationalId);

        public Task<CustomResponse<List<ProfessorDTO>>> GetProfessorsByDepartment(int departmentId,TakeSkipModel takeSkipModel);

        public Task<CustomResponse<List<ProfessorDTO>>> GetAllProfessors(TakeSkipModel takeSkipModel);

        public Task<CustomResponse<ProfessorDTO>> Add(ProfessorInputModel model);

        public Task<CustomResponse<bool>> Delete(int profId);

        public Task<CustomResponse<ProfessorDTO>> EditName(int profId,NameInputModel model);

        public Task<CustomResponse<ProfessorDTO>> EditHiringDate(int profId,DateTime date);

        public Task<CustomResponse<ProfessorDTO>> EditDepartment(int profId,int departmentId);

        public Task<CustomResponse<ProfessorDTO>> EditPhoneNumber(int profId, string newPhoneNumber);

        
    }
}

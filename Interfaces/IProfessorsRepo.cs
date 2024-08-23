using College_managemnt_system.ClientModels;
using College_managemnt_system.CustomResponse;
using College_managemnt_system.DTOS;

namespace College_managemnt_system.Interfaces
{
    public interface IProfessorsRepo
    {
        public Task<CustomResponse<IEnumerable<ProfessorDTO>>> GetProfessorsByDepartment(int departmentId,TakeSkipModel takeSkipModel);

        public Task<CustomResponse<IEnumerable<ProfessorDTO>>> GetAllProfessors(TakeSkipModel takeSkipModel);

        public Task<CustomResponse<ProfessorDTO>> Add(ProfessorInputModel model);

        public Task<CustomResponse<bool>> Delete(int profId);

        public Task<CustomResponse<ProfessorDTO>> EditName(EditNameInputModel model);

        public Task<CustomResponse<ProfessorDTO>> EditHiringDate(EditDateInputModel model);

        public Task<CustomResponse<ProfessorDTO>> EditDepartment(EditDepartmentInputModle modle);

        public Task<CustomResponse<ProfessorDTO>> EditPhoneNumber(EditPhoneNumberInputModel model);
    }
}

using College_managemnt_system.ClientModels;
using College_managemnt_system.CustomResponse;
using College_managemnt_system.DTOS;

namespace College_managemnt_system.Interfaces
{
    public interface ISemstersRepo
    {
        public Task<CustomResponse<IEnumerable<SemesterDTO>>> GetSemesters(TakeSkipModel takeSkipModel);
        public Task<CustomResponse<SemesterDTO>> GetSingleSemester(int semesterID);

        public Task<CustomResponse<SemesterDTO>> GetSemesterByNameYear(GetSemesterModel getSemesterModel);
        public Task<CustomResponse<SemesterDTO>> AddSemester(SemesterInputModel semesterInputModel);
        public Task<CustomResponse<SemesterDTO>>  EditEndDateSemester(EditDateModel editDateModel);
        public Task<CustomResponse<SemesterDTO>> EditStartDateSemester(EditDateModel editDateModel);

        public Task<CustomResponse<SemesterDTO>> EditActiveStatus(EditIsActiveModel editIsActiveModel);

        public Task<CustomResponse<bool>> DeleteSemester(int semesterID); //Critical DO NOT USE UNLESS NECESSARY

    }
}

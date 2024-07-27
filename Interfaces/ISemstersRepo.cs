using College_managemnt_system.ClientModels;
using College_managemnt_system.CustomResponse;
using College_managemnt_system.DTOS;

namespace College_managemnt_system.Interfaces
{
    public interface ISemstersRepo
    {
        public Task<CustomResponse<IEnumerable<SemestersDTO>>> GetSemesters(TakeSkipModel takeSkipModel);
        public Task<CustomResponse<SemestersDTO>> GetSingleSemester(int semesterID);

        public Task<CustomResponse<SemestersDTO>> GetSemesterByNameYear(GetSemesterModel getSemesterModel);
        public Task<CustomResponse<SemestersDTO>> AddSemester(SemesterInputModel semesterInputModel);
        public Task<CustomResponse<SemestersDTO>>  EditEndDateSemester(EditDateModel editDateModel);
        public Task<CustomResponse<SemestersDTO>> EditStartDateSemester(EditDateModel editDateModel);

        public Task<CustomResponse<SemestersDTO>> EditActiveStatus(EditIsActiveModel editIsActiveModel);

        public Task<CustomResponse<bool>> DeleteSemester(int semesterID); //Critical DO NOT USE UNLESS NECESSARY

    }
}

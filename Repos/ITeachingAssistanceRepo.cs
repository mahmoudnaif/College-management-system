using College_managemnt_system.ClientModels;
using College_managemnt_system.CustomResponse;
using College_managemnt_system.DTOS;
using College_managemnt_system.models;

namespace College_managemnt_system.Repos
{
    public interface ITeachingAssistanceRepo
    {
        public Task<CustomResponse<TeachingAssistanceDTO>> AddTa(TeachingAssistanceInputModel model);
        public Task<CustomResponse<bool>> RemoveTa(int taId);
        public Task<CustomResponse<TeachingAssistanceDTO>> EditName(int taId, NameInputModel model);
        public Task<CustomResponse<TeachingAssistanceDTO>> EditPhone(int taId,string newPhoneNumber);
        public Task<CustomResponse<TeachingAssistanceDTO>> EditHiringDate(int taId,DateTime hiringDate);
        public Task<CustomResponse<List<TeachingAssistanceDTO>>> GetAllTas(TakeSkipModel takeSkipModel);
        public Task<CustomResponse<List<TeachingAssistanceDTO>>> SearchTas(string searchQuery,TakeSkipModel takeSkipModel);
        public Task<CustomResponse<TeachingAssistanceDTO>> GetTaByNationalId(string nationalId);
    }
}

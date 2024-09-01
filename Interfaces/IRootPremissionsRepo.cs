using College_managemnt_system.ClientModels;
using College_managemnt_system.CustomResponse;

namespace College_managemnt_system.Interfaces
{
    public interface IRootPremissionsRepo //This repo will be responsible for managing premissions from a root user.
    {                                     //He can allow others to do certian operations for a specific amount of time using Redis cache.
        public Task<CustomResponse<bool>> Enable(String key, TimeCachedInputModel expirationDuration);
        public Task<CustomResponse<bool>> Enable(String key, DateTime expirationDate);
        public Task<CustomResponse<bool>> Disable(string key);
        public Task<bool> Check(string key);
        public Task<CustomResponse<bool>> CheckForEndPoint(string key);
    }
}
using College_managemnt_system.ClientModels;

namespace College_managemnt_system.Interfaces
{
    public interface IRootPremissionsRepo //This repo will be responsible for managing premissions from a root user.
    {                                     //He can allow others to do certian operations for a specific amount of time using Redis cache.
        public Task<bool> Enable(String key,String value, TimeCachedInputModel ExpirationDuration);
        public Task<bool> Enable(String key,String value, DateTime expirationDate);
        public Task<bool> Disable(string key);
        public Task<bool> Check(string key,string value);
    }
}

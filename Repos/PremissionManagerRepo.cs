using College_managemnt_system.ClientModels;
using College_managemnt_system.CustomResponse;
using College_managemnt_system.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Identity.Client;

namespace College_managemnt_system.Repos
{
    public class PremissionManagerRepo : IPremissionManagerRepo
    {
        private readonly IDistributedCache _distributedCache;

        public PremissionManagerRepo(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }
        public async Task<CustomResponse<bool>> Enable(string key, TimeCachedInputModel ExpirationDuration)
        {
            double totalMinutes = ExpirationDuration.weeks * 7 * 24 * 60 + ExpirationDuration.days*24*60 + ExpirationDuration.hours*60 + ExpirationDuration.minutes;
            if(totalMinutes == 0)
                return new CustomResponse<bool>(400,"Duration must be more than 0");

            try
            {
                await _distributedCache.SetStringAsync(key, "true", new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(totalMinutes)
                });

                return new CustomResponse<bool>(201, "Premission set successfully");
            }
            catch
            {
                return new CustomResponse<bool>(500, "Internal server error");
            }
        }
        public async Task<CustomResponse<bool>> Enable(string key, DateTime expirationDate)
        {
            if (expirationDate < DateTime.UtcNow)
                return new CustomResponse<bool>(400, "Date must be in the future");

            try
            {
                await _distributedCache.SetStringAsync(key, "true", new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = expirationDate
                });

                return new CustomResponse<bool>(201, "Premission set successfully");
            }
            catch
            {
                return new CustomResponse<bool>(500, "Internal server error");
            }
        }
        public async Task<bool> Check(string key)
        {
            try
            {
                return await _distributedCache.GetStringAsync(key) != null;
            }
            catch
            {
                return false;
            }
        }
        public async Task<CustomResponse<bool>> Disable(string key)
        {
            if (!await Check(key))
                return new CustomResponse<bool>(404, "Premission is alreadys disabaled");

            try
            {
                await _distributedCache.RemoveAsync(key);
                return new CustomResponse<bool>(200, "Premission removed successfully");
            }
            catch
            {
                return new CustomResponse<bool>(500, "Internal server error");
            }
        }

        public async Task<CustomResponse<bool>> CheckForEndPoint(string key)
        {
            if (await _distributedCache.GetStringAsync(key) == null)
                return new CustomResponse<bool>(403, "Premission denied");


            return new CustomResponse<bool>(200, "Premission granted");
        }
    }
}

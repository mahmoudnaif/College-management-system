using College_managemnt_system.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace College_managemnt_system.Repos
{
    public class BlackListTokensRepo : IBlackListTokensRepo
    {
        private readonly IMemoryCache _memoryCache;

        public BlackListTokensRepo(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        public Task BlacklistTokensAsync(int accountId, DateTime timestamp)
        {
            int timeCached = 3 * 60; ; 
          

            _memoryCache.Set(accountId, timestamp, new MemoryCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(timeCached)
            });
            return Task.CompletedTask;
        }

        public Task<bool> IsTokenBlacklisted(int accountId, DateTime issuedAt)
        {
            if (_memoryCache.TryGetValue(accountId, out DateTime blacklistTime))
            {
                return Task.FromResult(issuedAt <= blacklistTime);
            }
            return Task.FromResult(false);
        }
    }
}

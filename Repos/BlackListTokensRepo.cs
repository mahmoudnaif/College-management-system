using College_managemnt_system.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;

namespace College_managemnt_system.Repos
{
    public class BlackListTokensRepo : IBlackListTokensRepo
    {
        private readonly IDistributedCache _distributedCache;

        public BlackListTokensRepo(IDistributedCache distributedCache)
        {
            _distributedCache = distributedCache;
        }
        public async Task<bool> BlacklistTokensAsync(int accountId, DateTime timestamp)
        {
            int timeCached = 3 * 60; ;

            try
            {
                await _distributedCache.SetStringAsync(accountId.ToString(), timestamp.ToString("o"), new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(timeCached)
                });

                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> IsTokenBlacklisted(int accountId, DateTime issuedAt)
        {
            try
            {
                string dataAsString = await _distributedCache.GetStringAsync(accountId.ToString());
                if (dataAsString == null)
                    return false;

                DateTime blacklistTime = DateTime.Parse(dataAsString, null, System.Globalization.DateTimeStyles.RoundtripKind);

                return issuedAt <= blacklistTime;
            }
            catch
            {
                return true; //If redis some how failed all tokens will be treated as blacklisted.
                             //(Edit later or keep it if it meets up with the maximum securtiy specifications for a college management system).
            }
        }
    }
}

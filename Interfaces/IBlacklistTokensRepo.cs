namespace College_managemnt_system.Interfaces
{
    public interface IBlackListTokensRepo
    {
        public Task BlacklistTokensAsync(int accountId, DateTime timestamp);
        public Task<bool> IsTokenBlacklisted(int accountId, DateTime issuedAt);
    }
}

namespace College_managemnt_system.Interfaces
{
    public interface ITokensRepo
    {
        public  string generateLoginJWT(int id, string role, string key);
        public  string generateVerficationJWT(int id, string key);
        public  string generateChangePasswordJWT(int id, string key);
        public Task<int> IsTokenValid(string token); // going to need in the future if used SignalR library
    }
}

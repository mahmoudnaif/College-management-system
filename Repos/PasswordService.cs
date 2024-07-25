using College_managemnt_system.Interfaces;
using College_managemnt_system.models;
using Microsoft.AspNetCore.Identity;

namespace College_managemnt_system.Repos
{
    public class PasswordService : IPasswordService
    {
       
            private readonly PasswordHasher<Account> _passwordHasher;

            public PasswordService()
            {
                _passwordHasher = new PasswordHasher<Account>();
            }

            public byte[] HashPassword(Account user, string password)
            {
                string hashedPassword = _passwordHasher.HashPassword(user, password);
                return Convert.FromBase64String(hashedPassword);
            }

            public bool VerifyPassword(Account user, string providedPassword)
            {
                string storedPasswordHash = Convert.ToBase64String(user.Password);
                var result = _passwordHasher.VerifyHashedPassword(user, storedPasswordHash, providedPassword);
                return result == PasswordVerificationResult.Success;
            }
        

    }
}

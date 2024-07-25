using College_managemnt_system.models;
using Microsoft.AspNetCore.Identity;

namespace College_managemnt_system.Interfaces
{
    public interface IPasswordService
    {
      

            public byte[] HashPassword(Account user, string password);

            public bool VerifyPassword(Account user, string providedPassword);
        

    }
}

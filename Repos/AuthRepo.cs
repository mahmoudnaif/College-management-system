using College_managemnt_system.ClientModels;
using College_managemnt_system.CustomResponse;
using College_managemnt_system.Interfaces;
using College_managemnt_system.models;
using College_managemnt_system.Repos.Utilities;
using Microsoft.EntityFrameworkCore;

namespace College_managemnt_system.Repos
{
    public class AuthRepo : IAuthRepo
    {
        private readonly CollegeDBContext _context;
        private readonly IPasswordService _passwordService;
        private readonly IConfiguration _configuration;
        private readonly UtilitiesRepo _utilitiesRepo;
        private readonly ITokensRepo _tokensRepo;

        public AuthRepo(CollegeDBContext collegeDBContext, IPasswordService passwordService, IConfiguration configuration, UtilitiesRepo utilitiesRepo, ITokensRepo tokensRepo)
        {
            _context = collegeDBContext;
            _passwordService = passwordService;
            _configuration = configuration;
            _utilitiesRepo = utilitiesRepo;
            _tokensRepo = tokensRepo;
        }


        public async Task<CustomResponse<bool>> CreateAccountAsync(SiqnupModel siqnupModel)
        {
            if (!_utilitiesRepo.IsValidEmail(siqnupModel.email))
                return new CustomResponse<bool>(400, "Invalid email");

            if (!_utilitiesRepo.IsValidPassword(siqnupModel.password))
                return new CustomResponse<bool>(400, "Password must be at least 8 character long with at least 1 capital letter one small and a number.");


            if (siqnupModel.password != siqnupModel.repeatPassword)
                return new CustomResponse<bool>(400, "Passwords does not match");



            var emailUser = _context.Accounts.FirstOrDefault(acc => acc.Email.ToLower() == siqnupModel.email.ToLower());
            if (emailUser != null)
                return new CustomResponse<bool>(409, "Email already exists");


            Account account = new Account();

            account.Email = siqnupModel.email;
            account.Password = _passwordService.HashPassword(account, siqnupModel.password);
            account.DateCreated = DateTime.UtcNow;
            account.Role = "user";


            try
            {
                _context.Accounts.Add(account);
                await _context.SaveChangesAsync();
                return new CustomResponse<bool>(201, "Account Created Successfully");
            }
            catch
            {
                return new CustomResponse<bool>(500, "Something went wrong! Please try again later"); ;
            }


        }

        public async Task<CustomResponse<object>> Signin(Siqninmodel siqninmodel)
        {
            Account account = null;

            if (_utilitiesRepo.IsValidEmail(siqninmodel.email))
            {
                account = _context.Accounts.FirstOrDefault(acc => acc.Email.ToLower() == siqninmodel.email.ToLower());
            }
            else
            {
                return new CustomResponse<Object>(400, "Email is not valid");
            }

            if (account == null)
            {
                return new CustomResponse<Object>(404, "Account was not found");
            }

            if (!_passwordService.VerifyPassword(account, siqninmodel.password))
                return new CustomResponse<Object>(401, "Check your password");

            string JWTToken = _tokensRepo.generateLoginJWT((int)account.AccountId, account.Role, _configuration["Jwt:Key"]);



            return new CustomResponse<Object>(201, "Logged in successfully", new { Token = JWTToken });

        }
    }
}

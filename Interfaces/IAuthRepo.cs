using College_managemnt_system.CustomResponse;
using  College_managemnt_system.ClientModels;


namespace College_managemnt_system.Interfaces
{
    public interface IAuthRepo
    {
        public Task<CustomResponse<bool>> CreateAccountAsync(SiqnupModel siqnupModel);

        public Task<CustomResponse<Object>> Signin(Siqninmodel siqninmodel);
    }
}

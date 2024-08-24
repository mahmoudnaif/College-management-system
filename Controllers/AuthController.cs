using College_managemnt_system.ClientModels;
using College_managemnt_system.CustomResponse;
using College_managemnt_system.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace College_managemnt_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthRepo _authRepo;

        public AuthController(IAuthRepo authRepo)
        {
            _authRepo = authRepo;
        }

        [HttpPost("/signup")]
        [ProducesResponseType(201)]
        [Authorize(Roles ="root,admin")]
        public async Task<IActionResult> Siqnup([FromBody] SiqnupModel signupModel)
        {
            CustomResponse<bool> customResponse = await _authRepo.CreateAccountAsync(signupModel);

            return StatusCode(customResponse.responseCode, customResponse);
        }

        [HttpPost("/siginin")]
        [ProducesResponseType(201)]
        public async Task<IActionResult> Login([FromBody] Siqninmodel siqninmodel)
        {
            CustomResponse<Object> customResponse = await _authRepo.Signin(siqninmodel);

            return StatusCode(customResponse.responseCode, customResponse);
        }

    }
}

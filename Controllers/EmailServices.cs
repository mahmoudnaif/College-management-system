using College_managemnt_system.ClientModels;
using College_managemnt_system.CustomResponse;
using College_managemnt_system.Interfaces.Email;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace College_managemnt_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailServices : Controller
    {
        private readonly IEmailRepo _emailServicesRepo;

        public EmailServices(IEmailRepo emailServicesRepo)
        {
            _emailServicesRepo = emailServicesRepo;
        }

        [HttpPost("SendVerficationEmail")]
        [Authorize]
        public async Task<IActionResult> SendVerficationEmail()
        {
            int id;
            try
            {
                id = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            }
            catch
            {
                return StatusCode(400, new CustomResponse<bool>(400, "the sent token doesn't include the account id"));

            }
            CustomResponse<bool> customResponse = await _emailServicesRepo.SendVerificationEmail(id);

            return StatusCode(customResponse.responseCode, customResponse);
        }

        [HttpPut("VerifyEmail")]
        [Authorize]
        public async Task<IActionResult> VerifyEmail()
        {
            int id;
            try
            {
                id = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "accountId")?.Value);
                if (User.Claims.FirstOrDefault(c => c.Type == "Verify")?.Value != "true")
                    throw new Exception();
            }
            catch
            {
                return StatusCode(400, new CustomResponse<bool>(400, "Can't verify email using that token"));
            }
            CustomResponse<bool> customResponse = await _emailServicesRepo.VerifyAccount(id);

            return StatusCode(customResponse.responseCode, customResponse);
        }


        [HttpPost("SendChangePasswordEmail")]
        public async Task<IActionResult> SendChangePasswordEmail([FromBody] string email)
        {

            CustomResponse<bool> customResponse = await _emailServicesRepo.SendPasswordChangeEmail(email);

            return StatusCode(customResponse.responseCode, customResponse);
        }


        [HttpPut("ChangePassword")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordEmailModel changePasswordEmailModel)
        {
            int id;
            try
            {
                id = int.Parse(User.Claims.FirstOrDefault(c => c.Type == "accountId")?.Value);
                if (User.Claims.FirstOrDefault(c => c.Type == "changepassword")?.Value != "true")
                    throw new Exception();
            }
            catch
            {
                return StatusCode(400, new CustomResponse<bool>(400, "Can't verify email using that token"));
            }
            CustomResponse<bool> customResponse = await _emailServicesRepo.ChangePassword(id, changePasswordEmailModel.newPassword, changePasswordEmailModel.repeatNewPassword);

            return StatusCode(customResponse.responseCode, customResponse);
        }



    }
}

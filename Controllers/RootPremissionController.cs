using College_managemnt_system.ClientModels;
using College_managemnt_system.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace College_managemnt_system.Controllers
{
    [Route("api/premissions")]
    [ApiController]
    public class RootPremissionController : Controller
    {
        private readonly IRootPremissionsRepo _rootPremissionsRepo;

        public RootPremissionController(IRootPremissionsRepo rootPremissionsRepo)
        {
            _rootPremissionsRepo = rootPremissionsRepo;
        }
        [HttpPost("registerStudetns")]
        [Authorize(Roles ="root")]
        public async Task<IActionResult> EnableRegestringStudetns([FromBody] TimeCachedInputModel timeCachedInputModel )
        {
            var response = await _rootPremissionsRepo.Enable("registerStudetns", timeCachedInputModel);

            return StatusCode(response.responseCode, response);
        }

        [HttpPost("registerStudetns/expirationDate")]
        [Authorize(Roles = "root")]
        public async Task<IActionResult> EnableRegestringStudetns([FromBody] DateTime expirationDate)
        {
            var response = await _rootPremissionsRepo.Enable("registerStudetns", expirationDate);

            return StatusCode(response.responseCode, response);
        } 

        [HttpDelete("registerStudetns")]
        [Authorize(Roles = "root")]
        public async Task<IActionResult> DisableRegestringStudetns()
        {
            var response = await _rootPremissionsRepo.Disable("registerStudetns");

            return StatusCode(response.responseCode, response);
        } 
        
        [HttpGet("registerStudetns")]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> CheckRegestringStudetns() 
        {
            var response = await _rootPremissionsRepo.CheckForEndPoint("registerStudetns");

            return StatusCode(response.responseCode, response);
        }

        [HttpPost("registerCourses")]
        [Authorize(Roles = "root")]
        public async Task<IActionResult> EnableRegestringCourses([FromBody] TimeCachedInputModel timeCachedInputModel)
        {
            var response = await _rootPremissionsRepo.Enable("registerCourses", timeCachedInputModel);

            return StatusCode(response.responseCode, response);
        }

        [HttpPost("registerCourses/expirationDate")]
        [Authorize(Roles = "root")]
        public async Task<IActionResult> EnableRegestringCourses([FromBody] DateTime expirationDate)
        {
            var response = await _rootPremissionsRepo.Enable("registerCourses", expirationDate);

            return StatusCode(response.responseCode, response);
        }

        [HttpDelete("registerCourses")]
        [Authorize(Roles = "root")]
        public async Task<IActionResult> DisableRegestringCourses()
        {
            var response = await _rootPremissionsRepo.Disable("registerCourses");

            return StatusCode(response.responseCode, response);
        }

        [HttpGet("registerCourses")]
        [Authorize(Roles = "root,admin,student,ta")]
        public async Task<IActionResult> CheckRegestringCourses()
        {
            var response = await _rootPremissionsRepo.CheckForEndPoint("registerCourses");

            return StatusCode(response.responseCode, response);
        }
    }
}

using College_managemnt_system.ClientModels;
using College_managemnt_system.Interfaces;
using College_managemnt_system.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace College_managemnt_system.Controllers
{
    [Route("api/root/premissions")]
    [ApiController]
    public class RootPremissionController : Controller //Impmented but not used yet to block / allow certian operations from admins and students.
    {
        private readonly PremissionUtilsRepo _premissionUtilsRepo;

        public RootPremissionController(PremissionUtilsRepo premissionUtilsRepo)
        {

            _premissionUtilsRepo = premissionUtilsRepo;
        }

        //Start of Register students.


        [HttpPost("registerStudetns")]
        [Authorize(Roles ="root")]
        public async Task<IActionResult> EnableRegestringStudetns([FromBody] TimeCachedInputModel timeCachedInputModel )
        {
            var response = await _premissionUtilsRepo.EnableRegestringStudetns(timeCachedInputModel);

            return StatusCode(response.responseCode, response);
        }

        [HttpPost("registerStudetns/expirationDate")]
        [Authorize(Roles = "root")]
        public async Task<IActionResult> EnableRegestringStudetns([FromBody] DateTime expirationDate)
        {
            var response = await _premissionUtilsRepo.EnableRegestringStudetns(expirationDate);

            return StatusCode(response.responseCode, response);
        } 

        [HttpDelete("registerStudetns")]
        [Authorize(Roles = "root")]
        public async Task<IActionResult> DisableRegestringStudetns()
        {
            var response = await _premissionUtilsRepo.DisableRegestringStudetns();

            return StatusCode(response.responseCode, response);
        } 
        
        [HttpGet("registerStudetns")]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> CheckRegestringStudetns() 
        {
            var response = await _premissionUtilsRepo.CheckRegestringStudetnsEndPoint();

            return StatusCode(response.responseCode, response);
        }


        //End of register students


        //start of register courses


        [HttpPost("admin/registerCourses")]
        [Authorize(Roles = "root")]
        public async Task<IActionResult> EnableRegestringCourses([FromBody] TimeCachedInputModel timeCachedInputModel)
        {
            var response = await _premissionUtilsRepo.EnableRegestringCourses(timeCachedInputModel);

            return StatusCode(response.responseCode, response);
        }

        [HttpPost("admin/registerCourses/expirationDate")]
        [Authorize(Roles = "root")]
        public async Task<IActionResult> EnableRegestringCourses([FromBody] DateTime expirationDate)
        {
            var response = await _premissionUtilsRepo.EnableRegestringCourses(expirationDate);

            return StatusCode(response.responseCode, response);
        }

        [HttpDelete("admin/registerCourses")]
        [Authorize(Roles = "root")]
        public async Task<IActionResult> DisableRegestringCourses()
        {
            var response = await _premissionUtilsRepo.DisableRegestringCourses();

            return StatusCode(response.responseCode, response);
        }

        [HttpGet("admin/registerCourses")]
        [Authorize(Roles = "root,admin,ta")]
        public async Task<IActionResult> CheckRegestringCourses()
        {
            var response = await _premissionUtilsRepo.CheckRegestringCoursesEndPoint();

            return StatusCode(response.responseCode, response);
        }


        //end of register courses
    }
}

using College_managemnt_system.ClientModels;
using College_managemnt_system.Interfaces;
using College_managemnt_system.Repos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace College_managemnt_system.Controllers
{
    [Route("api/root/premissions")]
    [ApiController]
    public class RootPremissionController : Controller 
    {
        private readonly PremissionUtilsRepo _premissionUtilsRepo;

        public RootPremissionController(PremissionUtilsRepo premissionUtilsRepo)
        {

            _premissionUtilsRepo = premissionUtilsRepo;
        }

        //Start of Register students.


        [HttpPost("admin/students/register")]
        [Authorize(Roles ="root")]
        public async Task<IActionResult> EnableRegestringStudetns([FromBody] TimeCachedInputModel timeCachedInputModel )
        {
            var response = await _premissionUtilsRepo.EnableRegestringStudetns(timeCachedInputModel);

            return StatusCode(response.responseCode, response);
        }

        [HttpPost("admin/students/register/expirationDate")]
        [Authorize(Roles = "root")]
        public async Task<IActionResult> EnableRegestringStudetns([FromBody] DateTime expirationDate)
        {
            var response = await _premissionUtilsRepo.EnableRegestringStudetns(expirationDate);

            return StatusCode(response.responseCode, response);
        } 

        [HttpDelete("admin/students/register")]
        [Authorize(Roles = "root")]
        public async Task<IActionResult> DisableRegestringStudetns()
        {
            var response = await _premissionUtilsRepo.DisableRegestringStudetns();

            return StatusCode(response.responseCode, response);
        } 
        
        [HttpGet("admin/students/register")]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> CheckRegestringStudetns() 
        {
            var response = await _premissionUtilsRepo.CheckRegestringStudetnsEndPoint();

            return StatusCode(response.responseCode, response);
        }


        //End of register students


        //start of register courses


        [HttpPost("admin/courses/register")]
        [Authorize(Roles = "root")]
        public async Task<IActionResult> EnableRegestringCourses([FromBody] TimeCachedInputModel timeCachedInputModel)
        {
            var response = await _premissionUtilsRepo.EnableRegestringCourses(timeCachedInputModel);

            return StatusCode(response.responseCode, response);
        }

        [HttpPost("admin/courses/register/expirationDate")]
        [Authorize(Roles = "root")]
        public async Task<IActionResult> EnableRegestringCourses([FromBody] DateTime expirationDate)
        {
            var response = await _premissionUtilsRepo.EnableRegestringCourses(expirationDate);

            return StatusCode(response.responseCode, response);
        }

        [HttpDelete("admin/courses/register")]
        [Authorize(Roles = "root")]
        public async Task<IActionResult> DisableRegestringCourses()
        {
            var response = await _premissionUtilsRepo.DisableRegestringCourses();

            return StatusCode(response.responseCode, response);
        }

        [HttpGet("admin/courses/register")]
        [Authorize(Roles = "root,admin,ta")]
        public async Task<IActionResult> CheckRegestringCourses()
        {
            var response = await _premissionUtilsRepo.CheckRegestringCoursesEndPoint();

            return StatusCode(response.responseCode, response);
        }


        //End of register courses

        //Start of Dropping Courses

        [HttpPost("admin/course/drop")]
        [Authorize(Roles = "root")]
        public async Task<IActionResult> EnableDroppingCourses([FromBody] TimeCachedInputModel timeCachedInputModel)
        {
            var response = await _premissionUtilsRepo.EnableDroppingCourses(timeCachedInputModel);

            return StatusCode(response.responseCode, response);
        }

        [HttpPost("admin/course/drop/expirationDate")]
        [Authorize(Roles = "root")]
        public async Task<IActionResult> EnableDroppingCourses([FromBody] DateTime expirationDate)
        {
            var response = await _premissionUtilsRepo.EnableDroppingCourses(expirationDate);

            return StatusCode(response.responseCode, response);
        }

        [HttpDelete("admin/course/drop")]
        [Authorize(Roles = "root")]
        public async Task<IActionResult> DisableDroppingCourses()
        {
            var response = await _premissionUtilsRepo.DisableDroppingCourses();

            return StatusCode(response.responseCode, response);
        }

        [HttpGet("admin/course/drop")]
        [Authorize(Roles = "root,admin,ta")]
        public async Task<IActionResult> CheckDroppingCourses()
        {
            var response = await _premissionUtilsRepo.CheckDroppingCoursesEndPoint();

            return StatusCode(response.responseCode, response);
        }


        //End of Dropping Courses 
        
        
        //Start of withdrawing Courses

        [HttpPost("admin/course/withdraw")]
        [Authorize(Roles = "root")]
        public async Task<IActionResult> EnablewithdrawingCourses([FromBody] TimeCachedInputModel timeCachedInputModel)
        {
            var response = await _premissionUtilsRepo.EnableWithdrawingCourses(timeCachedInputModel);

            return StatusCode(response.responseCode, response);
        }

        [HttpPost("admin/course/withdraw/expirationDate")]
        [Authorize(Roles = "root")]
        public async Task<IActionResult> EnablewithdrawingCourses([FromBody] DateTime expirationDate)
        {
            var response = await _premissionUtilsRepo.EnableWithdrawingCourses(expirationDate);

            return StatusCode(response.responseCode, response);
        }

        [HttpDelete("admin/course/withdraw")]
        [Authorize(Roles = "root")]
        public async Task<IActionResult> DisablewithdrawingCourses()
        {
            var response = await _premissionUtilsRepo.DisableWithdrawingCourses();

            return StatusCode(response.responseCode, response);
        }

        [HttpGet("admin/course/withdraw")]
        [Authorize(Roles = "root,admin,ta")]
        public async Task<IActionResult> CheckWithdrawingCourses()
        {
            var response = await _premissionUtilsRepo.CheckWithdrawingCoursesEndPoint();

            return StatusCode(response.responseCode, response);
        }


        //End of withdrawing Courses


        //Start of grade Courses

        [HttpPost("admin/course/Grade")]
        [Authorize(Roles = "root")]
        public async Task<IActionResult> EnableGradingCourses([FromBody] TimeCachedInputModel timeCachedInputModel)
        {
            var response = await _premissionUtilsRepo.EnableGradingCourses(timeCachedInputModel);

            return StatusCode(response.responseCode, response);
        }

        [HttpPost("admin/course/Grade/expirationDate")]
        [Authorize(Roles = "root")]
        public async Task<IActionResult> EnablegradeingCourses([FromBody] DateTime expirationDate)
        {
            var response = await _premissionUtilsRepo.EnableGradingCourses(expirationDate);

            return StatusCode(response.responseCode, response);
        }

        [HttpDelete("admin/course/Grade")]
        [Authorize(Roles = "root")]
        public async Task<IActionResult> DisableGradingCourses()
        {
            var response = await _premissionUtilsRepo.DisableGradingCourses();

            return StatusCode(response.responseCode, response);
        }

        [HttpGet("admin/course/Grade")]
        [Authorize(Roles = "root,admin,ta")]
        public async Task<IActionResult> CheckGradingCourses()
        {
            var response = await _premissionUtilsRepo.CheckGradingCoursesEndPoint();

            return StatusCode(response.responseCode, response);
        }


        //End of grade Courses

    }
}

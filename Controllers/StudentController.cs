using College_managemnt_system.ClientModels;
using College_managemnt_system.Interfaces;
using College_managemnt_system.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Identity.Client;

namespace College_managemnt_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : Controller 
    {
        private readonly IStudentRepo _studentRepo;

        public StudentController(IStudentRepo studentRepo)
        {
            _studentRepo = studentRepo;
        }

        [HttpGet("year/{year}")]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> GetStudentsByYear(int year, [FromQuery] TakeSkipModel takeSkipModel)
        {
            var result = await _studentRepo.GetStudentSByYear(year, takeSkipModel);

            return StatusCode(result.responseCode, result);
        }

        [HttpGet("email/{email}")]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> GetStudentByEmail(string email)
        {
            var result = await _studentRepo.GetStudentByEmail(email);

            return StatusCode(result.responseCode, result);
        }

        [HttpGet("search/{searchQuery}")]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> GetStudentsByYear(string searchQuery, [FromQuery] TakeSkipModel takeSkipModel)
        {
            var result = await _studentRepo.SearchStudentsByName(searchQuery, takeSkipModel);

            return StatusCode(result.responseCode, result);
        }

        [HttpPost]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> Add([FromBody] StudentInputModel model)
        {
            var result = await _studentRepo.Add(model);

            return StatusCode(result.responseCode, result);
        }

        [HttpDelete("{studentId}")]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> Delete(int studentId)
        {
            var result = await _studentRepo.Delete(studentId);

            return StatusCode(result.responseCode, result);
        }

        [HttpPut("{studentId}/name")]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> EditName(int studentId, [FromBody] FullNameInputModel model)
        {
            var result = await _studentRepo.EditName(studentId,model);

            return StatusCode(result.responseCode, result);
        }
        
        [HttpPut("{studentId}/phone")]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> EditPhone(int studentId, [FromBody] string phoneNumber)
        {
            var result = await _studentRepo.EditPhone(studentId,phoneNumber);

            return StatusCode(result.responseCode, result);
        }


    }
}

using College_managemnt_system.ClientModels;
using College_managemnt_system.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace College_managemnt_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfessorController : Controller
    {
        private readonly IProfessorsRepo _professorsRepo;

        public ProfessorController(IProfessorsRepo professorsRepo) {
            _professorsRepo = professorsRepo;
        }

        [HttpGet("All")]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> GetAllProfessors([FromQuery] TakeSkipModel takeSkipModel)
        {
            var response = await _professorsRepo.GetAllProfessors(takeSkipModel);

            return StatusCode(response.responseCode,response);
        }

        [HttpPost]
        [Authorize(Roles ="root,admin")]
        public async Task<IActionResult> Add(ProfessorInputModel model)
        {
            var response = await _professorsRepo.Add(model);

            return StatusCode(response.responseCode, response);
        }

        [HttpDelete("{profId}")]
        [Authorize(Roles ="root,admin")]
        public async Task<IActionResult> Delete(int profId)
        {
            var response = await _professorsRepo.Delete(profId);

            return StatusCode(response.responseCode, response);
        }

        [HttpPut("{profId}/Name")]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> EditName(int profId, [FromBody] NameInputModel model)
        {
          
            var response = await _professorsRepo.EditName(profId,model);

            return StatusCode(response.responseCode, response);
        }

        [HttpPut("{profId}/hiringdate")]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> HiringDate(int profId,[FromBody] DateTime date)
        {
            
            
            var response = await _professorsRepo.EditHiringDate(profId,date);

            return StatusCode(response.responseCode, response);
        }

        [HttpPut("{profId}/phone")]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> Phone(int profId,[FromBody]string phoneNumber)
        {
            var response = await _professorsRepo.EditPhoneNumber(profId,phoneNumber);

            return StatusCode(response.responseCode, response);
        }

        [HttpPut("{profId}/department")]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> Department(int profId,[FromBody]int departmentId)
        {
            var response = await _professorsRepo.EditDepartment(profId,departmentId);

            return StatusCode(response.responseCode, response);
        }
    }
}

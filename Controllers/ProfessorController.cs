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

        [HttpDelete]
        [Authorize(Roles ="root,admin")]
        public async Task<IActionResult> Delete(int profId)
        {
            var response = await _professorsRepo.Delete(profId);

            return StatusCode(response.responseCode, response);
        }

        [HttpPut("Name")]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> EditName(EditNameInputModel model)
        {
            var response = await _professorsRepo.EditName(model);

            return StatusCode(response.responseCode, response);
        }

        [HttpPut("hiringdate")]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> HiringDate(EditDateInputModel model)
        {
            var response = await _professorsRepo.EditHiringDate(model);

            return StatusCode(response.responseCode, response);
        }

        [HttpPut("phone")]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> Phone(EditPhoneNumberInputModel model)
        {
            var response = await _professorsRepo.EditPhoneNumber(model);

            return StatusCode(response.responseCode, response);
        }

        [HttpPut("department")]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> Department(EditDepartmentInputModle model)
        {
            var response = await _professorsRepo.EditDepartment(model);

            return StatusCode(response.responseCode, response);
        }
    }
}

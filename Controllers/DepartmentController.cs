using College_managemnt_system.ClientModels;
using College_managemnt_system.Interfaces;
using College_managemnt_system.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace College_managemnt_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : Controller
    {
        private readonly IDepartmentsRepo _departmentsRepo;
        private readonly IProfessorsRepo _professorsRepo;

        public DepartmentController(IDepartmentsRepo departmentsRepo, IProfessorsRepo professorsRepo)
        {
            _departmentsRepo = departmentsRepo;
            _professorsRepo = professorsRepo;
        }


        [HttpGet("All")]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> GetAllDepartments() { 
            
            var response =await _departmentsRepo.GetDepartments();

            return StatusCode(response.responseCode, response);

        }



        [HttpPost]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> AddDepartment([FromBody] string DepartmentName)
        {
            var response = await _departmentsRepo.AddDepartment(DepartmentName);

            return StatusCode(response.responseCode, response);
        }

        [HttpPut("{departmentId}/Name")]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> EditDepartmentName(int departmentId, [FromBody]string newName)
        {
            EditDepartmentNameModel editDepartmentNameModel = new EditDepartmentNameModel()
            {
                departmentId = departmentId,
                newName = newName
            };
            var response = await _departmentsRepo.EditDepartmentName(editDepartmentNameModel);

            return StatusCode(response.responseCode, response);
        }


        [HttpDelete("Delete")]
        [Authorize(Roles = "root")]
        public async Task<IActionResult> DeleteDepartment([FromBody] int departmentId)
        {
            var response = await _departmentsRepo.DeleteDepartment(departmentId);

            return StatusCode(response.responseCode, response);
        }

        [HttpGet("{id}/Professors")]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> GetProfessorsByDepartment(int id, [FromQuery] TakeSkipModel takeSkipModel)
        {
            var response = await _professorsRepo.GetProfessorsByDepartment(id, takeSkipModel);

            return StatusCode(response.responseCode, response);
        }


    }
}

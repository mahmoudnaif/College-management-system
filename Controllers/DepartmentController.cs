using College_managemnt_system.ClientModels;
using College_managemnt_system.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace College_managemnt_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DepartmentController : Controller
    {
        private readonly IDepartmentsRepo _departmentsRepo;

        public DepartmentController(IDepartmentsRepo departmentsRepo)
        {
            _departmentsRepo = departmentsRepo;
        }


        [HttpGet("GetAll")]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> GetAllDepartments() { 
            
            var response =await _departmentsRepo.GetDepartments();

            return StatusCode(response.responseCode, response);

        }



        [HttpPost("Add")]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> AddDepartment([FromBody] string DepartmentName)
        {
            var response = await _departmentsRepo.AddDepartment(DepartmentName);

            return StatusCode(response.responseCode, response);
        }

        [HttpPut("EditName")]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> EditDepartmentName([FromBody] EditDepartmentNameModel editDepartmentNameModel)
        {
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




        }
}

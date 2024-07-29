using College_managemnt_system.ClientModels;
using College_managemnt_system.Interfaces;
using College_managemnt_system.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace College_managemnt_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrereqController : Controller
    {
        private readonly CollegeDBContext _context;
        private readonly IPrereqsCoursesRepo _prereqsCoursesRepo;

        public PrereqController(CollegeDBContext context, IPrereqsCoursesRepo prereqsCoursesRepo)
        {
            _context = context;
            _prereqsCoursesRepo = prereqsCoursesRepo;
        }
        [HttpGet("{id}")]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> GetPrereqs(int id)
        {
            var result = await _prereqsCoursesRepo.GetPrereqsCourses(id);

            return StatusCode(result.responseCode, result);
        }

        [HttpPost()]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> AddPrereqs(PrereqsInputModel prereqsInputModel)
        {
            var result = await _prereqsCoursesRepo.AddPrereqsCourse(prereqsInputModel);

            return StatusCode(result.responseCode, result);
        }

        [HttpDelete()]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> DeletePrereqs(PrereqsInputModel prereqsInputModel)
        {
            var result = await _prereqsCoursesRepo.RemovePrereqsCourse(prereqsInputModel);

            return StatusCode(result.responseCode, result);
        }
    }
}

using College_managemnt_system.models;
using Microsoft.AspNetCore.Mvc;

namespace College_managemnt_system.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly CollegeDBContext _context;

        public WeatherForecastController(CollegeDBContext context)
        {
            _context = context;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IActionResult Get(int courseId)
        {
            IEnumerable<Course> prereqsCoursesDTO = _context.Prereqs.Where(P => P.CourseId == courseId).Select(P => _context.Courses.SingleOrDefault(C => C.CourseId == P.PrereqCourseId));


            return Ok(prereqsCoursesDTO);
        }
    }
}

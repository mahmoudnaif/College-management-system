using College_managemnt_system.ClientModels;
using College_managemnt_system.Interfaces;
using College_managemnt_system.models;
using College_managemnt_system.Repos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace College_managemnt_system.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private readonly CollegeDBContext _context;
        private readonly IRegisterSemesterCoursesRepo _registerSemesterCoursesRepo;
        

        public WeatherForecastController(CollegeDBContext context, IRegisterSemesterCoursesRepo registerSemesterCoursesRepo)
        {
            _context = context;
            _registerSemesterCoursesRepo = registerSemesterCoursesRepo;
            
        }

        [HttpPost("GetWeatherForecast/{studentId}/{groupId}/{bypassRules}")]
        public async Task<IActionResult> Get(int studentId,[FromBody] List<int> courseIds,int groupId,bool bypassRules)
        {
            var response = await _registerSemesterCoursesRepo.RegisterCourses_SchedulesByGroup(studentId,courseIds,groupId,bypassRules);

            return StatusCode(response.responseCode,response);
        }


        [HttpPost("GetWeatherForecast")]
        public async Task<IActionResult> Get(List<int> courseIds)
        {
            var response = await _registerSemesterCoursesRepo.GetAvailableSchedule(courseIds);

            return StatusCode(response.responseCode, response);
        }
    }
}

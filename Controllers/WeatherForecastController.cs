using College_managemnt_system.ClientModels;
using College_managemnt_system.models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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

        [HttpPost(Name = "GetWeatherForecast")]
        public async Task<IActionResult> Get([FromBody]SemesterInputModel semesterInputModel)
        {
            await _context.Database.ExecuteSqlRawAsync(@"UPDATE s
SET 
    s.CGPA = COALESCE(cgpa_results.CGPA, 0),
    s.totalHours = COALESCE(cgpa_results.TotalCreditHours, 0)
FROM 
    Students s
INNER JOIN 
    (SELECT 
        sc.StudentID,
        SUM(c.Credits) AS TotalCreditHours,
        SUM(CASE 
                WHEN sc.Grade = 'A' THEN 4 * c.Credits
                WHEN sc.Grade = 'B' THEN 3 * c.Credits
                WHEN sc.Grade = 'C' THEN 2 * c.Credits
                WHEN sc.Grade = 'D' THEN 1 * c.Credits
                ELSE 0
            END) / CAST(SUM(c.Credits) AS FLOAT) AS CGPA
    FROM 
        StudentCourses sc
    INNER JOIN 
        Coursesemesters cs ON sc.CourseSemesterID = cs.CourseSemesterID
    INNER JOIN 
        Courses c ON cs.CourseID = c.CourseID
    WHERE 
        sc.IsFinished = 1
    GROUP BY 
        sc.StudentID
    ) AS cgpa_results 
    ON s.StudentID = cgpa_results.StudentID;
");
            return Ok();
        }
    }
}

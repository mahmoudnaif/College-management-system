using College_managemnt_system.ClientModels;
using College_managemnt_system.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace College_managemnt_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ScheduleController : Controller
    {
        private readonly ISchedulesRepo _schedulesRepo;

        public ScheduleController(ISchedulesRepo schedulesRepo)
        {
            _schedulesRepo = schedulesRepo;
        }


      /*  [HttpGet]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> GetScheduls([FromQuery] GetSchduelsBySemester model)
        {
            var response = await _schedulesRepo.GetScheduls(model.SemesterId,new TakeSkipModel() { skip = model.skip, take=model.take});

            return StatusCode(response.responseCode, response);

        } */  // migrated to the semester controller


        [HttpPost]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> Add([FromBody] SchedulesInputModel schedulesInputModel)
        {
            var response = await _schedulesRepo.Add(schedulesInputModel);

            return StatusCode(response.responseCode, response);

        }

        [HttpPut("time&place")]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> Edit([FromBody] EditScheduleTimeandPlace editScheduleTimeandPlace)
        {
            var response = await _schedulesRepo.EditTimeAndPlace(editScheduleTimeandPlace);

            return StatusCode(response.responseCode, response);

        }

        [HttpDelete]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> Delete([FromBody]int ScheduleId)
        {
            var response = await _schedulesRepo.Remove(ScheduleId);

            return StatusCode(response.responseCode, response);

        }


    }
}

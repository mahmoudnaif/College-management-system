using College_managemnt_system.ClientModels;
using College_managemnt_system.Interfaces;
using College_managemnt_system.models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace College_managemnt_system.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GroupController : Controller
    {
        private readonly IGroupsRepo _groupsRepo;
        private readonly ISchedulesGroupsRepo _schedulesGroupsRepo;

        public GroupController(IGroupsRepo groupsRepo,ISchedulesGroupsRepo schedulesGroupsRepo)
        {
            _groupsRepo = groupsRepo;
            _schedulesGroupsRepo = schedulesGroupsRepo;
        }

        [HttpPost]
        [Authorize(Roles = "admin,root")]
        public async Task<IActionResult> AddGroup([FromBody] GroupInputModel groupInputModel)
        {
            var result = await _groupsRepo.AddGroup(groupInputModel);

            return StatusCode(result.responseCode, result);
        }


        [HttpDelete("{groupId}")]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> DeleteGroup(int groupId)
        {
            var result = await _groupsRepo.DeleteGroup(groupId);

            return StatusCode(result.responseCode, result);
        }

        [HttpGet("{groupId}/schedule")]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> GetSchedule(int groupId)
        {
            var result = await _schedulesGroupsRepo.GetSchedulsByGroup(groupId);

            return StatusCode(result.responseCode, result);
        }

        [HttpDelete("{groupId}/schedule/{scheduleId}")]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> GetSchedule(int groupId, int scheduleId)
        {
            var result = await _schedulesGroupsRepo.RemoveScheduleFromGroup(groupId,scheduleId);

            return StatusCode(result.responseCode, result);
        }

        [HttpPost("{groupId}/schedule")]
        [Authorize(Roles = "root,admin")]
        public async Task<IActionResult> AddScheduletoGroup(int groupId, [FromBody]int scheduleId)
        {
            var result = await _schedulesGroupsRepo.AddScheduleToGroup(groupId, scheduleId);

            return StatusCode(result.responseCode, result);
        }



    }
}

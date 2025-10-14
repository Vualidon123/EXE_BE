using EXE_BE.Controllers.ViewModel;
using EXE_BE.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EXE_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrafficUsageController : ControllerBase
    {
        private readonly TrafficUsageService _trafficUsageService;
        private readonly UserActivitiesSerivce _userActivitiesSerivce;
        public TrafficUsageController(TrafficUsageService trafficUsageService, UserActivitiesSerivce userActivitiesSerivce)
        {
            _trafficUsageService = trafficUsageService;
            _userActivitiesSerivce = userActivitiesSerivce;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetTrafficUsageById(int id)
        {
            var trafficUsage = await _trafficUsageService.GetTrafficUsageByIdAsync(id);
            if (trafficUsage == null)
            {
                return NotFound();
            }
            return Ok(trafficUsage);
        }
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetTrafficUsageByUserId(int userId)
        {
            var trafficUsages = await _trafficUsageService.GetTrafficUsageByIdAsync(userId);
            return Ok(trafficUsages);
        }
        [HttpPost]
        public async Task<IActionResult> AddTrafficUsage([FromBody] TrafficUsageDto request)
        {
            var trafficUsage = request.ToEntity();
            var createdTrafficUsage = await _trafficUsageService.AddTrafficUsageAsync(trafficUsage);
            return CreatedAtAction(nameof(GetTrafficUsageById), new { id = createdTrafficUsage.Id }, createdTrafficUsage);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTrafficUsage(int id, [FromBody] TrafficUsageDto request)
        {
            if (id != request.Id)
            {
                return BadRequest();
            }
            var trafficUsage = request.ToEntity();
            await _trafficUsageService.UpdateTrafficUsageAsync(trafficUsage);
            await _userActivitiesSerivce.UpdateTotalCO2EmissionAsync(trafficUsage.ActivityId);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrafficUsage(int id)
        {
            await _trafficUsageService.DeleteTrafficUsageAsync(id);
            return NoContent();
        }

    }

}

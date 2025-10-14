using EXE_BE.Controllers.ViewModel;
using EXE_BE.Models;
using EXE_BE.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EXE_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PlasticUsageController : ControllerBase
    {
        private readonly PlasticUsageService _plasticUsageService;  
        private readonly UserActivitiesSerivce _userActivitiesSerivce;
        public PlasticUsageController(PlasticUsageService plasticUsageService, UserActivitiesSerivce userActivitiesSerivce)
        {
            _plasticUsageService = plasticUsageService;
            _userActivitiesSerivce = userActivitiesSerivce;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetPlasticUsageById(int id)
        {
            var plasticUsage = await _plasticUsageService.GetPlasticUsageByIdAsync(id);
            if (plasticUsage == null)
            {
                return NotFound();
            }
            return Ok(plasticUsage);
        }
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetPlasticUsageByUserId(int userId)
        {
            var plasticUsages = await _plasticUsageService.GetPlasticUsageByUserId(userId);
            return Ok(plasticUsages);
        }
        [HttpPost]
        public async Task<IActionResult> AddPlasticUsage([FromBody] PlasticUsageDto request)
        {
            var plasticUsage = request.ToEntity();
            var createdPlasticUsage = await _plasticUsageService.AddPlasticUsageAsync(plasticUsage);
            return CreatedAtAction(nameof(GetPlasticUsageById), new { id = createdPlasticUsage.Id }, createdPlasticUsage);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePlasticUsage(int id, [FromBody] PlasticUsageDto request)
        {
            if (id != request.Id)
            {
                return BadRequest();
            }
            var plasticUsage = request.ToEntity();
            await _plasticUsageService.UpdatePlasticUsageAsync(plasticUsage);
            await _userActivitiesSerivce.UpdateTotalCO2EmissionAsync(plasticUsage.ActivityId);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePlasticUsage(int id)
        {
            await _plasticUsageService.DeletePlasticUsageAsync(id);
            return NoContent();
        }


    }
}

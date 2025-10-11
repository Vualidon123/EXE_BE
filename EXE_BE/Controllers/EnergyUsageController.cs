using EXE_BE.Controllers.ViewModel;
using EXE_BE.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EXE_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnergyUsageController : ControllerBase
    {
        private readonly EnergyUsageService _energyUsageService;
        public EnergyUsageController(EnergyUsageService energyUsageService)
        {
            _energyUsageService = energyUsageService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllEnergyUsages()
        {
            var energyUsages = await _energyUsageService.GetEnergyUsages();
            return Ok(energyUsages);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetEnergyUsageById(int id)
        {
            var energyUsage = await _energyUsageService.GetEnergyUsageByIdAsync(id);
            if (energyUsage == null)
            {
                return NotFound();
            }
            return Ok(energyUsage);
        }
        //[HttpPost]
        //public async Task<IActionResult> AddEnergyUsage([FromBody] EnergyUsageDto request)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }
        //    var energyUsage= request.ToEntity();
        //    var createdEnergyUsage = await _energyUsageService.AddEnergyUsageAsync(energyUsage);
        //    return CreatedAtAction(nameof(GetEnergyUsageById), new { id = createdEnergyUsage.Id }, createdEnergyUsage);
        //}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateEnergyUsage(int id, [FromBody] EnergyUsageDto request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingEnergyUsage = await _energyUsageService.GetEnergyUsageByIdAsync(id);
            if (existingEnergyUsage == null)
                return NotFound();

            // 🔥 Update the tracked entity directly
            existingEnergyUsage.ActivityId = request.ActivityId;
            existingEnergyUsage.date = request.Date;
            existingEnergyUsage.electricityconsumption = request.ElectricityConsumption;
            existingEnergyUsage.CO2emission = request.CO2Emission;

            await _energyUsageService.UpdateEnergyUsageAsync(existingEnergyUsage);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEnergyUsage(int id)
        {
            var existingEnergyUsage = await _energyUsageService.GetEnergyUsageByIdAsync(id);
            if (existingEnergyUsage == null)
            {
                return NotFound();
            }
            await _energyUsageService.DeleteEnergyUsageAsync(id);
            return NoContent();
        }
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetEnergyUsageByUserId(int userId)
        {
            var energyUsages = await _energyUsageService.GetEnergyUsageByUserId(userId);
            return Ok(energyUsages);
        }
    }
}

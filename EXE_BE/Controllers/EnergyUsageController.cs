using EXE_BE.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EXE_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnergyUsageController : ControllerBase
    {
        private readonly EnergyUsageSerivce _energyUsageService;
        public EnergyUsageController(EnergyUsageSerivce energyUsageService)
        {
            _energyUsageService = energyUsageService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAllEnergyUsages()
        {
            var energyUsages = await _energyUsageService.GetEnergyUsages();
            return Ok(energyUsages);
        }
    }
}

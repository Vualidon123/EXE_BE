using EXE_BE.Controllers.ViewModel;
using EXE_BE.Models;
using EXE_BE.Models.ItemList;
using EXE_BE.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EXE_BE.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserActivitiesController : ControllerBase
    {
        private readonly UserActivitiesSerivce _userActivitiesService;
        private readonly EnergyUsageSerivce _energyUsage;
        private readonly FoodUsageService _foodUsage;
        private readonly PlasticUsageService _plasticUsage;
        private readonly TrafficUsageSerivce _trafficUsage;
        public UserActivitiesController(UserActivitiesSerivce userActivitiesService, TrafficUsageSerivce trafficUsage, EnergyUsageSerivce energyUsageSerivce, PlasticUsageService plasticUsageService, FoodUsageService foodUsageService)
        {
            _userActivitiesService = userActivitiesService;
            _trafficUsage = trafficUsage;
            _energyUsage = energyUsageSerivce;
            _plasticUsage = plasticUsageService;
            _foodUsage = foodUsageService;
        }
        [HttpPost]
        public async Task<IActionResult> AddUserActivities([FromBody] UserActivitiesInputModel input)
        {
            // Map input to entity
            var userActivities = new UserActivities
            {
                UserId = 1,//Claim ,
                Date = DateTime.UtcNow,
                PlasticUsage = new PlasticUsage
                {
                    date = DateTime.UtcNow,
                    CO2emission=0,
                    PlasticItems = input.PlasticUsage.PlasticItems.Select(item => new PlasticItem
                    {
                        PlasticCategory = item.PlasticCategory,
                        Weight = item.Weight
                    }).ToList()
                },
                TrafficUsage = new TrafficUsage
                {
                    date = DateTime.UtcNow,
                    distance = input.TrafficUsage.Distance,
                    trafficCategory = input.TrafficUsage.TrafficCategory,

                }, 
                FoodUsage = new FoodUsage
                {
                    date = DateTime.UtcNow,


                    FoodItems = input.FoodUsage.FoodItems.Select(item => new FoodItem
                    {
                        FoodCategory = item.FoodCategory,
                        Weight = item.Weight
                    }).ToList()
                },
                EnergyUsage = new EnergyUsage
                {
                    date = DateTime.UtcNow,
                    electricityconsumption = input.EnergyUsage.ElectricityConsumption,

                }
            };

            // Call the service
            /*  await _trafficUsage.AddTrafficUsageAsync(userActivities.TrafficUsage);
              await _energyUsage.AddEnergyUsageAsync(userActivities.EnergyUsage);
              await _plasticUsage.AddPlasticUsageAsync(userActivities.PlasticUsage);
              await _foodUsage.AddFoodUsageAsync(userActivities.FoodUsage);*/

            await _userActivitiesService.AddUserActivitiesAsync(userActivities);
            return Ok(new { message = "User activity added successfully" });
        }

        [HttpGet]
        public async Task<IActionResult> GetUserActivities()
        {
            var activities = await _userActivitiesService.GetAcitvicties();

            return Ok(activities);
        }
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            await _userActivitiesService.DeleteUserActivitiesAsync(id);
            return Ok();
        }
    }
}

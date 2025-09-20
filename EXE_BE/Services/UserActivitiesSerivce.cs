using EXE_BE.Data.Repository;
using EXE_BE.Models;

namespace EXE_BE.Services
{
    public class UserActivitiesSerivce
    {
        private readonly UserActivitiesRepository _userActivitiesRepository;
        private readonly EnergyUsageSerivce _energyUsageRepository;
        private readonly PlasticUsageService _plasticUsageRepository;
        private readonly FoodUsageService _foodUsageRepository;
        private readonly TrafficUsageSerivce _trafficUsageRepository;
        public UserActivitiesSerivce(UserActivitiesRepository userActivitiesRepository, EnergyUsageSerivce energyUsage, FoodUsageService foodUsageRepository, PlasticUsageService plasticUsageRepository, TrafficUsageSerivce trafficUsageRepository )
        {
            _userActivitiesRepository = userActivitiesRepository;
            _energyUsageRepository= energyUsage;
            _foodUsageRepository= foodUsageRepository;
            _plasticUsageRepository= plasticUsageRepository;
            _trafficUsageRepository= trafficUsageRepository;
        }
        public async Task<UserActivities> AddUserActivitiesAsync(UserActivities userActivities)
        {
            // 1. Create UserActivities
            var createdActivity = await _userActivitiesRepository.AddUserActivitiesAsync(userActivities);

            // 2. Link children to parent (set FK)
            if (userActivities.FoodUsage != null)
            {
                userActivities.FoodUsage.ActivityId = createdActivity.Id;
                await _foodUsageRepository.UpdateFoodUsageAsync(userActivities.FoodUsage);
            }

            if (userActivities.PlasticUsage != null)
            {
                userActivities.PlasticUsage.ActivityId = createdActivity.Id;
                await _plasticUsageRepository.UpdatePlasticUsageAsync(userActivities.PlasticUsage);
            }

            if (userActivities.TrafficUsage != null)
            {
                userActivities.TrafficUsage.ActivityId = createdActivity.Id;
                await _trafficUsageRepository.UpdateTrafficUsageAsync(userActivities.TrafficUsage);
            }

            if (userActivities.EnergyUsage != null)
            {
                userActivities.EnergyUsage.ActivityId = createdActivity.Id;
                await _energyUsageRepository.UpdateEnergyUsageAsync(userActivities.EnergyUsage);
            }
            userActivities.TotalCO2Emission= (userActivities.PlasticUsage?.CO2emission ?? 0) +
                                             (userActivities.FoodUsage?.CO2emission ?? 0) +
                                             (userActivities.TrafficUsage?.CO2emission ?? 0) +
                                             (userActivities.EnergyUsage?.CO2emission ?? 0);
            await _userActivitiesRepository.UpdateUserActivitiesAsync(userActivities);
            return createdActivity;
        }
        public async Task<UserActivities?> GetUserActivitiesByIdAsync(int id)
        {
            return await _userActivitiesRepository.GetUserActivitiesByIdAsync(id);
        }
       public async Task<List<UserActivities>> GetAcitvicties()
       { 
            return await _userActivitiesRepository.GetAllUserActivitiesAsync(); 
       }
       public async Task UpdateUserActivitiesAsync(UserActivities userActivities)
        {
            await _userActivitiesRepository.UpdateUserActivitiesAsync(userActivities);
        }
        public async Task DeleteUserActivitiesAsync(int id)
        {
            await _userActivitiesRepository.DeleteUserActivitiesAsync(id);
        }

        public async Task<List<User>> LeaderBoard()
        {
            return await _userActivitiesRepository.GetLeaderboardByCO2Emission();
        }
    }   

}

using EXE_BE.Controllers.ViewModel;
using EXE_BE.Data.Repository;
using EXE_BE.Models;
using Microsoft.EntityFrameworkCore;

namespace EXE_BE.Services
{
    public class UserActivitiesSerivce
    {
        private readonly UserActivitiesRepository _userActivitiesRepository;
        private readonly EnergyUsageService _energyUsageRepository;
        private readonly PlasticUsageService _plasticUsageRepository;
        private readonly FoodUsageService _foodUsageRepository;
        private readonly TrafficUsageService _trafficUsageRepository;
        public UserActivitiesSerivce(UserActivitiesRepository userActivitiesRepository, EnergyUsageService energyUsage, FoodUsageService foodUsageRepository, PlasticUsageService plasticUsageRepository, TrafficUsageService trafficUsageRepository)
        {
            _userActivitiesRepository = userActivitiesRepository;
            _energyUsageRepository = energyUsage;
            _foodUsageRepository = foodUsageRepository;
            _plasticUsageRepository = plasticUsageRepository;
            _trafficUsageRepository = trafficUsageRepository;
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
            userActivities.TotalCO2Emission = (userActivities.PlasticUsage?.CO2emission ?? 0) +
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
        public async Task<List<UserActivities>> GetUserActivitiesByUserIdAsync(int userId)
        {
            return await _userActivitiesRepository.GetUserActivitiesByUserIdAsync(userId);
        }
        public async Task UpdateUserActivitiesAsync(UserActivities userActivities)
        {
            await _userActivitiesRepository.UpdateUserActivitiesAsync(userActivities);
        }
        public async Task DeleteUserActivitiesAsync(int id)
        {
            await _userActivitiesRepository.DeleteUserActivitiesAsync(id);
        }
        public async Task<List<UserActivities>> GetUserActivitiesAsync()
        {
            return await _userActivitiesRepository.GetUserActivitiesAsync();
        }
        public async Task<List<UserLeaderboardDto>> LeaderBoard()
        {
            // Get users from repo
            var users = await _userActivitiesRepository.GetLeaderboardByCO2Emission();

            // Convert to DTOs
            var leaderboard = users.Select(u => new UserLeaderboardDto
            {
                UserName = u.UserName,
                TotalCO2Emission = u.UserActivities?
                    .Sum(a => (float?)a.TotalCO2Emission) ?? 0f
            }).ToList();

            return leaderboard;
        }
        public async Task UpdateTotalCO2EmissionAsync(int userActivitiesId)
        {
            await _userActivitiesRepository.UpdateTotalCO2EmissionAsync(userActivitiesId);
        }

    }
}
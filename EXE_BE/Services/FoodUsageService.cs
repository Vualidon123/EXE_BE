using EXE_BE.Data.Repository;
using EXE_BE.Models;

namespace EXE_BE.Services
{
    public class FoodUsageService
    {
        private readonly FoodUsageRepository _foodUsageRepository;
        public FoodUsageService(FoodUsageRepository foodUsageRepository)
        {
            _foodUsageRepository = foodUsageRepository;
        }
        public async Task<FoodUsage> AddFoodUsageAsync(FoodUsage foodUsage)
        {
            return await _foodUsageRepository.AddFoodUsageAsync(foodUsage);
        }
        public async Task<FoodUsage?> GetFoodUsageByIdAsync(int id)
        {
            return await _foodUsageRepository.GetFoodUsageByIdAsync(id);
        }
        public async Task<List<FoodUsage>> GetFoodUsageByUserId(int userId)
        {
            return await _foodUsageRepository.GetFoodUsageByUserId(userId);
        }
        public async Task UpdateFoodUsageAsync(FoodUsage foodUsage)
        {
            await _foodUsageRepository.UpdateFoodUsageAsync(foodUsage);
        }

        public async Task DeleteFoodUsageAsync(int id)
        {
            await _foodUsageRepository.DeleteFoodUsageAsync(id);
        }
    }
}

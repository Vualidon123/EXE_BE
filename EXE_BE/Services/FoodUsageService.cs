using EXE_BE.Data;
using EXE_BE.Data.Repository;
using EXE_BE.Models;

namespace EXE_BE.Services
{
    public class FoodUsageService
    {
        private readonly FoodUsageRepository _foodUsageRepository;
        private readonly FoodItemRepository _foodItemRepository;
        private readonly CategorySelect _categorySelect;
        public FoodUsageService(FoodUsageRepository foodUsageRepository, FoodItemRepository foodItemRepository,CategorySelect categorySelect)
        {
            _foodUsageRepository = foodUsageRepository;
            _foodItemRepository = foodItemRepository;
            _categorySelect = categorySelect;
        }
        public async Task<FoodUsage> AddFoodUsageAsync(FoodUsage foodUsage)
        {
            float totalCO2 = 0;
            foreach (var item in foodUsage.FoodItems)
            {
                await _foodItemRepository.AddFoodItemAsync(item);
                totalCO2+=  _categorySelect.FoodCO2Emission(item)* item.Weight;
            }
            foodUsage.CO2emission = totalCO2;
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
            float totalCO2 = 0;
            foreach (var item in foodUsage.FoodItems)
            {
                await _foodItemRepository.UpdateFoodItemAsync(item);
                totalCO2 += _categorySelect.FoodCO2Emission(item) * item.Weight;
            }
            foodUsage.CO2emission = totalCO2;
            await _foodUsageRepository.UpdateFoodUsageAsync(foodUsage);
        }

        public async Task DeleteFoodUsageAsync(int id)
        {
            await _foodUsageRepository.DeleteFoodUsageAsync(id);
        }
    }
}

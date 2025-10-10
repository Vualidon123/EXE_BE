using EXE_BE.Data;
using EXE_BE.Data.Repository;
using EXE_BE.Models;

namespace EXE_BE.Services
{
    public class FoodUsageService
    {
        private readonly FoodUsageRepository _foodUsageRepository;
        private readonly CategorySelect CategorySelect = new CategorySelect();

        public FoodUsageService(FoodUsageRepository foodUsageRepository)
        {
            _foodUsageRepository = foodUsageRepository;
        }

        public async Task<FoodUsage> AddFoodUsageAsync(FoodUsage foodUsage)
        {
            float totalCO2 = 0;
            if (foodUsage.FoodItems != null)
            {
                foreach (var item in foodUsage.FoodItems)
                {
                    totalCO2 += CategorySelect.FoodCO2Emission(item) * item.Weight;
                }
            }
            foodUsage.CO2emission = totalCO2;
            return await _foodUsageRepository.CreateAsync(foodUsage);
        }

        public async Task<FoodUsage?> GetFoodUsageByIdAsync(int id)
        {
            return await _foodUsageRepository.GetByIdAsync(id);
        }

        //public async Task<List<FoodUsage>> GetFoodUsageByUserId(int userId)
        //{
        //    return await _foodUsageRepository.GetByUserIdAsync(userId);
        //}

        public async Task UpdateFoodUsageAsync(FoodUsage foodUsage)
        {
            float totalCO2 = 0;
            if (foodUsage.FoodItems != null)
            {
                foreach (var item in foodUsage.FoodItems)
                {
                    totalCO2 += CategorySelect.FoodCO2Emission(item) * item.Weight;
                }
            }
            foodUsage.CO2emission = totalCO2;
            await _foodUsageRepository.UpdateAsync(foodUsage);
        }

        public async Task DeleteFoodUsageAsync(int id)
        {
            await _foodUsageRepository.DeleteAsync(id);
        }
    }
}

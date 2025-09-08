using EXE_BE.Models;
using EXE_BE.Models.ItemList;
using Microsoft.EntityFrameworkCore;

namespace EXE_BE.Data.Repository
{
    public class FoodUsageRepository
    {
        private readonly AppDbContext _context;

        public FoodUsageRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<FoodUsage> AddFoodUsageAsync(FoodUsage foodUsage)
        {
            float co2PerKg;
            switch (foodUsage.FoodItems?.FirstOrDefault()?.FoodCategory)
            {
                case food_category.Beef:
                    co2PerKg = 27.0f;
                    break;
                case food_category.Lamb:
                    co2PerKg = 39.2f;
                    break;
                case food_category.Pork:
                    co2PerKg = 12.1f;
                    break;
                case food_category.Chicken:
                    co2PerKg = 6.9f;
                    break;
                case food_category.Fish:
                    co2PerKg = 6.1f;
                    break;
                case food_category.Eggs:
                    co2PerKg = 4.8f;
                    break;
                case food_category.Rice:
                    co2PerKg = 4.0f;
                    break;
                case food_category.Vegetables:
                    co2PerKg = 2.0f;
                    break;
                case food_category.Others:
                    co2PerKg = 5.0f; // Average value for other food items
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(foodUsage.FoodItems), "Unknown food category");
            }


            _context.FoodUsages.Add(foodUsage);
            foreach (var item in foodUsage.FoodItems)
            {
                _context.FoodItems.Add(item);
            }
            await _context.SaveChangesAsync();
            return foodUsage;
        }

        public async Task<FoodUsage?> GetFoodUsageByIdAsync(int id)
        {
            return await _context.FoodUsages.FindAsync(id);
        }
        public async Task<List<FoodUsage>> GetFoodUsageByUserId(int userId)
        {
            return await _context.FoodUsages.
                Include(x => x.FoodItems).
                Where(x => x.UserActivities.UserId == userId).
                ToListAsync();
        }

        public async Task UpdateFoodUsageAsync(FoodUsage foodUsage)
        {
            _context.FoodUsages.Update(foodUsage);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteFoodUsageAsync(int id)
        {
            var foodUsage = await _context.FoodUsages.FindAsync(id);
            if (foodUsage != null)
            {
                _context.FoodUsages.Remove(foodUsage);
                await _context.SaveChangesAsync();
            }
        }

    }
}

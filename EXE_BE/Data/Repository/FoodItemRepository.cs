using EXE_BE.Models.ItemList;

namespace EXE_BE.Data.Repository
{
    public class FoodItemRepository
    {
       private readonly AppDbContext _context;
        public FoodItemRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<FoodItem> AddFoodItemAsync(FoodItem foodItem)
        {
            _context.FoodItems.Add(foodItem);
            await _context.SaveChangesAsync();
            return foodItem;
        }
        public async Task<FoodItem?> GetFoodItemByIdAsync(int id)
        {
            return await _context.FoodItems.FindAsync(id);
        }
        public async Task UpdateFoodItemAsync(FoodItem foodItem)
        {
            _context.FoodItems.Update(foodItem);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteFoodItemAsync(int id)
        {
            var foodItem = await _context.FoodItems.FindAsync(id);
            if (foodItem != null)
            {
                _context.FoodItems.Remove(foodItem);
                await _context.SaveChangesAsync();
            }
        }
        /*public async Task<List<FoodItem>> GetFoodItemsByUserId(int userId)
        {
            return await _context.FoodItems.
                Where(x => x.FoodUsage.UserActivities.UserId == userId).
                ToListAsync();
        }
        public async Task<List<FoodItem>> GetFoodItems()
        {
            return await _context.FoodItems.
                
                ToListAsync();
        }*/
    }
}

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

        public async Task<FoodUsage> GetByIdAsync(int id)
        {
            return await _context.FoodUsages
                .Include(fu => fu.FoodItems)
                .FirstOrDefaultAsync(fu => fu.Id == id);
        }

        public async Task<List<FoodUsage>> GetAllAsync()
        {
            return await _context.FoodUsages
                .Include(fu => fu.FoodItems)
                .ToListAsync();
        }

        public async Task<FoodUsage> CreateAsync(FoodUsage entity)
        {
            _context.FoodUsages.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<FoodUsage> UpdateAsync(FoodUsage entity)
        {
            _context.FoodUsages.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.FoodUsages.FindAsync(id);
            if (entity == null) return false;

            _context.FoodUsages.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

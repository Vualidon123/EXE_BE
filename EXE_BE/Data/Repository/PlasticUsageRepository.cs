using EXE_BE.Models;
using EXE_BE.Models.ItemList;
using Microsoft.EntityFrameworkCore;

namespace EXE_BE.Data.Repository
{
    public class PlasticUsageRepository
    {
        private readonly AppDbContext _context;

        public PlasticUsageRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<PlasticUsage> GetByIdAsync(int id)
        {
            return await _context.PlasticUsages
                .Include(pu => pu.PlasticItems)
                .FirstOrDefaultAsync(pu => pu.Id == id);
        }

        public async Task<List<PlasticUsage>> GetAllAsync()
        {
            return await _context.PlasticUsages
                .Include(pu => pu.PlasticItems)
                .ToListAsync();
        }

        public async Task<PlasticUsage> CreateAsync(PlasticUsage entity)
        {
            _context.PlasticUsages.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<PlasticUsage> UpdateAsync(PlasticUsage entity)
        {
            _context.PlasticUsages.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.PlasticUsages.FindAsync(id);
            if (entity == null) return false;

            _context.PlasticUsages.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<PlasticUsage> GetByUserIdAsync(int userId)
        {
            return await _context.PlasticUsages
                .FirstOrDefaultAsync(pu => pu.UserActivities.UserId == userId);
        }
    }
}

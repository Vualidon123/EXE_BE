using EXE_BE.Models;
using Microsoft.EntityFrameworkCore;

namespace EXE_BE.Data.Repository
{
    public class TrafficUsageRepository 
    {
        private readonly AppDbContext _context;

        public TrafficUsageRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<TrafficUsage> GetByIdAsync(int id)
        {
            return await _context.TrafficUsages
                .FirstOrDefaultAsync(tu => tu.Id == id);
        }

        public async Task<List<TrafficUsage>> GetAllAsync()
        {
            return await _context.TrafficUsages.ToListAsync();
        }

        public async Task<TrafficUsage> CreateAsync(TrafficUsage entity)
        {
            _context.TrafficUsages.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<TrafficUsage> UpdateAsync(TrafficUsage entity)
        {
            _context.TrafficUsages.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.TrafficUsages.FindAsync(id);
            if (entity == null) return false;

            _context.TrafficUsages.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

using EXE_BE.Models;
using Microsoft.EntityFrameworkCore;

namespace EXE_BE.Data.Repository
{
    public class EnergyUsageRepository
    {
        private readonly AppDbContext _context;

        public EnergyUsageRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<EnergyUsage> GetByIdAsync(int id)
        {
            return await _context.EnergyUsages
                .FirstOrDefaultAsync(eu => eu.Id == id);
        }

        public async Task<List<EnergyUsage>> GetAllAsync()
        {
            return await _context.EnergyUsages.ToListAsync();
        }

        public async Task<EnergyUsage> CreateAsync(EnergyUsage entity)
        {
            _context.EnergyUsages.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task UpdateAsync(EnergyUsage entity)
        {
            _context.EnergyUsages.Update(entity);
            await _context.SaveChangesAsync();
           
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _context.EnergyUsages.FindAsync(id);
            if (entity == null) return false;

            _context.EnergyUsages.Remove(entity);
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<EnergyUsage> GetByUserIdAsync(int userId)
        {
            return await _context.EnergyUsages
                .FirstOrDefaultAsync(eu => eu.UserActivities.UserId == userId);
        }
    }
}

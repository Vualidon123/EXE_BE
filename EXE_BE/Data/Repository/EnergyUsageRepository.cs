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
        public async Task<EnergyUsage> AddEnergyUsageAsync(EnergyUsage energyUsage)
        {
            
            await _context.SaveChangesAsync();
            return energyUsage;
        }
        public async Task<EnergyUsage?> GetEnergyUsageByIdAsync(int id)
        {
            return await _context.EnergyUsages.FindAsync(id);
        }
        public async Task UpdateEnergyUsageAsync(EnergyUsage energyUsage)
        {
            _context.EnergyUsages.Update(energyUsage);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteEnergyUsageAsync(int id)
        {
            var energyUsage = await _context.EnergyUsages.FindAsync(id);
            if (energyUsage != null)
            {
                _context.EnergyUsages.Remove(energyUsage);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<List<EnergyUsage>> GetEnergyUsageByUserId(int userId)
        {
            return await _context.EnergyUsages.
                Where(x => x.UserActivities.UserId == userId).
                ToListAsync();
        }
        public async Task<List<EnergyUsage>> GetEnergyUsages()
        {
            return await _context.EnergyUsages.
                
                ToListAsync();
        }
    }
}

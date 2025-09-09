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
        public async Task<PlasticUsage> AddPlasticUsageAsync(PlasticUsage plasticUsage)
        {
           /* float co2PerKg; // Average CO2 emission per kg of plastic waste
            switch (plasticUsage.PlasticItems?.FirstOrDefault()?.PlasticCategory)
            {
                case plastic_category.PlasticBottle:
                    co2PerKg = 1.5f;
                    break;
                case plastic_category.PlasticBag:
                    co2PerKg = 3.0f;
                    break;
                case plastic_category.PlasticCup:
                    co2PerKg = 2.5f;
                    break;
                case plastic_category.PlasticStraw:
                    co2PerKg = 4.0f;
                    break;
                case plastic_category.PlasticContainer:
                    co2PerKg = 2.0f;
                    break;
                case plastic_category.Other:
                    co2PerKg = 3.0f; // Average value for other plastic items
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(plasticUsage.PlasticItems), "Unknown plastic category");
            }
            plasticUsage.CO2emission = ((float)co2PerKg * plasticUsage.PlasticItems.Sum(x => x.Weight));    */
            _context.PlasticUsages.Add(plasticUsage);
           /* foreach (var item in plasticUsage.PlasticItems)
            {
                _context.PlasticItems.Add(item);
            }*/
            await _context.SaveChangesAsync();
            return plasticUsage;
        }
        public async Task<PlasticUsage?> GetPlasticUsageByIdAsync(int id)
        {
            return await _context.PlasticUsages.FindAsync(id);
        }
        public async Task<List<PlasticUsage>> GetPlasticUsageByUserId(int userId)
        {
            return await _context.PlasticUsages.
                Include(x => x.PlasticItems).
                Where(x => x.UserActivities.UserId == userId).
                ToListAsync();
        }
        public async Task UpdatePlasticUsageAsync(PlasticUsage plasticUsage)
        {
            _context.PlasticUsages.Update(plasticUsage);
            await _context.SaveChangesAsync();
        }
    }

}

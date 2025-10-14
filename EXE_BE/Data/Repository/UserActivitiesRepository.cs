using EXE_BE.Models;
using Microsoft.EntityFrameworkCore;

namespace EXE_BE.Data.Repository
{
    public class UserActivitiesRepository
    {
        private readonly AppDbContext _context;

        public UserActivitiesRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<UserActivities> AddUserActivitiesAsync(UserActivities userActivities)
        {
            _context.UserActivities.Add(userActivities);
            await _context.SaveChangesAsync(); // This sets userActivities.Id

           /* // 2. Now assign ActivityId manually (just for consistency in memory)
            userActivities.PlasticUsage.ActivityId = userActivities.Id;
            userActivities.TrafficUsage.ActivityId = userActivities.Id;
            userActivities.FoodUsage.ActivityId = userActivities.Id;
            userActivities.EnergyUsage.ActivityId = userActivities.Id;

            // 3. Update the child entities
            _context.PlasticUsages.Update(userActivities.PlasticUsage);
            _context.TrafficUsages.Update(userActivities.TrafficUsage);
            _context.FoodUsages.Update(userActivities.FoodUsage);
            _context.EnergyUsages.Update(userActivities.EnergyUsage);*/

            await _context.SaveChangesAsync();
            return userActivities;
        }
        public async Task<UserActivities?> GetUserActivitiesByIdAsync(int id)
        {
            return await _context.UserActivities.
                 Include(c => c.PlasticUsage).
                Include(c => c.TrafficUsage).
                Include(c => c.FoodUsage).
                Include(c => c.EnergyUsage).
                FirstOrDefaultAsync(u => u.Id == id);
        }
        public async Task UpdateUserActivitiesAsync(UserActivities userActivities)
        {
            _context.UserActivities.Update(userActivities);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteUserActivitiesAsync(int id)
        {
            var userActivities = await _context.UserActivities.FindAsync(id);
            if (userActivities != null)
            {
                _context.UserActivities.Remove(userActivities);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<List<UserActivities>> GetUserActivitiesByUserIdAsync(int userId)
        {
            return await _context.UserActivities.
                Include(c => c.PlasticUsage).
                Include(c => c.TrafficUsage).
                Include(c => c.FoodUsage).
                Include(c => c.EnergyUsage).
                Where(u => u.UserId == userId).
                ToListAsync();
        }
        public async Task<List<UserActivities>> GetUserActivitiesAsync()
        {
            return await _context.UserActivities.
                Include(c => c.PlasticUsage).
                Include(c => c.TrafficUsage).
                Include(c => c.FoodUsage).
                Include(c => c.EnergyUsage).               
                ToListAsync();
        }



        public async Task<List<User>> GetLeaderboardByCO2Emission()
        {
            var sevenDaysAgo = DateTime.UtcNow.AddDays(-7);

            return await _context.Users
                .OrderBy(u => u.UserActivities
                    .Where(a => a.Date >= sevenDaysAgo)
                    .Sum(a => a.TotalCO2Emission))
                .Include(u => u.UserActivities.Where(a => a.Date >= sevenDaysAgo))
                .ToListAsync();
        }
        public async Task UpdateTotalCO2EmissionAsync(int userActivitiesId)
        {
            var userActivities = await _context.UserActivities
                .Include(u => u.PlasticUsage)
                .Include(u => u.TrafficUsage)
                .Include(u => u.FoodUsage)
                .Include(u => u.EnergyUsage)
                .FirstOrDefaultAsync(u => u.Id == userActivitiesId);

            if (userActivities != null)
            {
                userActivities.TotalCO2Emission =
                    (userActivities.PlasticUsage?.CO2emission ?? 0) +
                    (userActivities.TrafficUsage?.CO2emission ?? 0) +
                    (userActivities.FoodUsage?.CO2emission ?? 0) +
                    (userActivities.EnergyUsage?.CO2emission ?? 0);

                _context.UserActivities.Update(userActivities);
                await _context.SaveChangesAsync();
            }
        }

    }

}

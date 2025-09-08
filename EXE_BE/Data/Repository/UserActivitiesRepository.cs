using EXE_BE.Models;

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
            await _context.SaveChangesAsync();
            return userActivities;
        }
        public async Task<UserActivities?> GetUserActivitiesByIdAsync(int id)
        {
            return await _context.UserActivities.FindAsync(id);
        }
        public async Task UpdateUserActivitiesAsync(UserActivities userActivities)
        {
            _context.UserActivities.Update(userActivities);
            await _context.SaveChangesAsync();
        }
    }

}

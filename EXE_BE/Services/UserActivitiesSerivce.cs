using EXE_BE.Data.Repository;
using EXE_BE.Models;

namespace EXE_BE.Services
{
    public class UserActivitiesSerivce
    {
        private readonly UserActivitiesRepository _userActivitiesRepository;

        public UserActivitiesSerivce(UserActivitiesRepository userActivitiesRepository)
        {
            _userActivitiesRepository = userActivitiesRepository;
        }
        public async Task<UserActivities> AddUserActivitiesAsync(UserActivities userActivities)
        {
            return await _userActivitiesRepository.AddUserActivitiesAsync(userActivities);
        }
        public async Task<UserActivities?> GetUserActivitiesByIdAsync(int id)
        {
            return await _userActivitiesRepository.GetUserActivitiesByIdAsync(id);
        }
    }

}

using EXE_BE.Data.Repository;
using EXE_BE.Models;
using Microsoft.EntityFrameworkCore;

namespace EXE_BE.Services
{
    public class TrafficUsageSerivce
    {
        private readonly TrafficUsageRepository _trafficUsageRepository;
        public TrafficUsageSerivce(TrafficUsageRepository trafficUsageRepository)
        {
            _trafficUsageRepository = trafficUsageRepository;
        }
        public async Task<TrafficUsage> AddTrafficUsageAsync(TrafficUsage trafficUsage)
        {
            return await _trafficUsageRepository.AddTrafficUsageAsync(trafficUsage);
        }
        public async Task<TrafficUsage?> GetTrafficUsageByIdAsync(int id)
        {
            return await _trafficUsageRepository.GetTrafficUsageByIdAsync(id);
        }
        public async Task UpdateTrafficUsageAsync(TrafficUsage trafficUsage)
        {
            await _trafficUsageRepository.UpdateTrafficUsageAsync(trafficUsage);
        }
        public async Task DeleteTrafficUsageAsync(int id)
        {
            await _trafficUsageRepository.DeleteTrafficUsageAsync(id);
        }
    }
}

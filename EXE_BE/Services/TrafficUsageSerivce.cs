using EXE_BE.Data;
using EXE_BE.Data.Repository;
using EXE_BE.Models;
using Microsoft.EntityFrameworkCore;

namespace EXE_BE.Services
{
    public class TrafficUsageSerivce
    {
        private readonly TrafficUsageRepository _trafficUsageRepository;
        private readonly CategorySelect _categorySelect;
        public TrafficUsageSerivce(TrafficUsageRepository trafficUsageRepository,CategorySelect categorySelect)
        {
            _trafficUsageRepository = trafficUsageRepository;
            _categorySelect = categorySelect;
        }
        public async Task<TrafficUsage> AddTrafficUsageAsync(TrafficUsage trafficUsage)
        {         
            trafficUsage.CO2emission = _categorySelect.TrafficCO2Emission(trafficUsage)*trafficUsage.distance;
            return await _trafficUsageRepository.AddTrafficUsageAsync(trafficUsage);
        }
        public async Task<TrafficUsage?> GetTrafficUsageByIdAsync(int id)
        {
            return await _trafficUsageRepository.GetTrafficUsageByIdAsync(id);
        }
        public async Task UpdateTrafficUsageAsync(TrafficUsage trafficUsage)
        {
            trafficUsage.CO2emission = _categorySelect.TrafficCO2Emission(trafficUsage) * trafficUsage.distance;
            await _trafficUsageRepository.UpdateTrafficUsageAsync(trafficUsage);
        }
        public async Task DeleteTrafficUsageAsync(int id)
        {
            await _trafficUsageRepository.DeleteTrafficUsageAsync(id);
        }
    }
}

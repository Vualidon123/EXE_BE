using EXE_BE.Data;
using EXE_BE.Data.Repository;
using EXE_BE.Models;
using Microsoft.EntityFrameworkCore;

namespace EXE_BE.Services
{
    public class TrafficUsageService
    {
        private readonly TrafficUsageRepository _trafficUsageRepository;
        private readonly CategorySelect CategorySelect = new CategorySelect();
        public TrafficUsageService(TrafficUsageRepository trafficUsageRepository)
        {
            _trafficUsageRepository = trafficUsageRepository;
        }

        public async Task<TrafficUsage> AddTrafficUsageAsync(TrafficUsage trafficUsage)
        {
            trafficUsage.CO2emission = CategorySelect.TrafficCO2Emission(trafficUsage) * trafficUsage.distance;
            return await _trafficUsageRepository.CreateAsync(trafficUsage);
        }

        public async Task<TrafficUsage?> GetTrafficUsageByIdAsync(int id)
        {
            return await _trafficUsageRepository.GetByIdAsync(id);
        }

        public async Task UpdateTrafficUsageAsync(TrafficUsage trafficUsage)
        {
            trafficUsage.CO2emission = CategorySelect.TrafficCO2Emission(trafficUsage) * trafficUsage.distance;
            await _trafficUsageRepository.UpdateAsync(trafficUsage);
        }

        public async Task DeleteTrafficUsageAsync(int id)
        {
            await _trafficUsageRepository.DeleteAsync(id);
        }
    }
}

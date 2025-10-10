using EXE_BE.Data.Repository;
using EXE_BE.Models;
using Microsoft.EntityFrameworkCore;

namespace EXE_BE.Services
{
    public class EnergyUsageService
    {
        private readonly EnergyUsageRepository _energyUsageRepository;
        private readonly UserActivitiesRepository _userActivitiesRepository;

        public EnergyUsageService(EnergyUsageRepository energyUsageRepository, UserActivitiesRepository userActivitiesRepository)
        {
            _energyUsageRepository = energyUsageRepository;
            _userActivitiesRepository = userActivitiesRepository;
        }

       

        public async Task<EnergyUsage?> GetEnergyUsageByIdAsync(int id)
        {
            return await _energyUsageRepository.GetByIdAsync(id);
        }

        public async Task UpdateEnergyUsageAsync(EnergyUsage energyUsage)
        {
            // Validate ActivityId          
            energyUsage.CO2emission = energyUsage.electricityconsumption * 0.6766f;
            await _energyUsageRepository.UpdateAsync(energyUsage);
        }

        public async Task DeleteEnergyUsageAsync(int id)
        {
            await _energyUsageRepository.DeleteAsync(id);
        }

        public async Task<EnergyUsage> GetEnergyUsageByUserId(int userId)
        {
            return await _energyUsageRepository.GetByUserIdAsync(userId);
        }

        public async Task<List<EnergyUsage>> GetEnergyUsages()
        {
            return await _energyUsageRepository.GetAllAsync();
        }
    }

}

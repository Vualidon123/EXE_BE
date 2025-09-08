using EXE_BE.Data.Repository;
using EXE_BE.Models;
using Microsoft.EntityFrameworkCore;

namespace EXE_BE.Services
{
    public class EnergyUsageSerivce
    {
        private readonly EnergyUsageRepository _energyUsageRepository;
        public EnergyUsageSerivce(EnergyUsageRepository energyUsageRepository)
        {
            _energyUsageRepository = energyUsageRepository;
        }
        public async Task<EnergyUsage> AddEnergyUsageAsync(EnergyUsage energyUsage)
        {
          return  await _energyUsageRepository.AddEnergyUsageAsync(energyUsage);
        }
        public async Task<EnergyUsage?> GetEnergyUsageByIdAsync(int id)
        {
            return await _energyUsageRepository.GetEnergyUsageByIdAsync(id);
        }
        public async Task UpdateEnergyUsageAsync(EnergyUsage energyUsage)
        {
             await _energyUsageRepository.UpdateEnergyUsageAsync(energyUsage);
        }
        public async Task DeleteEnergyUsageAsync(int id)
        {
             await _energyUsageRepository.DeleteEnergyUsageAsync(id);
        }
        public async Task<List<EnergyUsage>> GetEnergyUsageByUserId(int userId)
        {
            return await _energyUsageRepository.GetEnergyUsageByUserId(userId);
        }
    }

}

using EXE_BE.Data;
using EXE_BE.Data.Repository;
using EXE_BE.Models;
using Microsoft.EntityFrameworkCore;

namespace EXE_BE.Services
{
    public class PlasticUsageService
    {
        private readonly PlasticUsageRepository _plasticUsageRepository;
        private readonly CategorySelect CategorySelect = new CategorySelect();
        public PlasticUsageService(PlasticUsageRepository plasticUsageRepository)
        {
            _plasticUsageRepository = plasticUsageRepository;
        }

        public async Task<PlasticUsage> AddPlasticUsageAsync(PlasticUsage plasticUsage)
        {
            float totalCO2 = 0;
            if (plasticUsage.PlasticItems != null)
            {
                foreach (var item in plasticUsage.PlasticItems)
                {
                    totalCO2 += CategorySelect.PlasticCO2Emission(item) * item.Weight;
                }
            }
            plasticUsage.CO2emission = totalCO2;
            return await _plasticUsageRepository.CreateAsync(plasticUsage);
        }

        public async Task<PlasticUsage?> GetPlasticUsageByIdAsync(int id)
        {
            return await _plasticUsageRepository.GetByIdAsync(id);
        }

        public async Task<PlasticUsage> GetPlasticUsageByUserId(int userId)
        {
            return await _plasticUsageRepository.GetByUserIdAsync(userId);
        }

        public async Task UpdatePlasticUsageAsync(PlasticUsage plasticUsage)
        {
            float totalCO2 = 0;
            if (plasticUsage.PlasticItems != null)
            {
                foreach (var item in plasticUsage.PlasticItems)
                {
                    totalCO2 += CategorySelect.PlasticCO2Emission(item) * item.Weight;
                }
            }
            plasticUsage.CO2emission = totalCO2;
            await _plasticUsageRepository.UpdateAsync(plasticUsage);
        }

        public async Task DeletePlasticUsageAsync(int id)
        {
            await _plasticUsageRepository.DeleteAsync(id);
        }
    }
}

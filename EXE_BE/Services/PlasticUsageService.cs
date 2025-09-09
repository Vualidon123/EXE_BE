using EXE_BE.Data;
using EXE_BE.Data.Repository;
using EXE_BE.Models;
using Microsoft.EntityFrameworkCore;

namespace EXE_BE.Services
{
    public class PlasticUsageService
    {
        private readonly PlasticUsageRepository _plasticUsageRepository;
        private readonly PlasticItemRepository _plasticItemRepository;
        private readonly CategorySelect _categorySelect;    
        public PlasticUsageService(PlasticUsageRepository plasticUsageRepository, PlasticItemRepository plasticItemRepository,CategorySelect categorySelect)
        {
            _plasticUsageRepository = plasticUsageRepository;
            _plasticItemRepository = plasticItemRepository;
            _categorySelect = categorySelect;
        }
        public async Task<PlasticUsage> AddPlasticUsageAsync(PlasticUsage plasticUsage)
        {
            float totalCO2=0;
            foreach (var item in plasticUsage.PlasticItems)
            {
                await _plasticItemRepository.AddPlasticItemAsync(item);
                totalCO2 += _categorySelect.PlasticCO2Emission(item)*item.Weight;
            }
            plasticUsage.CO2emission=totalCO2;
            return await _plasticUsageRepository.AddPlasticUsageAsync(plasticUsage);
        }
        public async Task<PlasticUsage?> GetPlasticUsageByIdAsync(int id)
        {
            return await _plasticUsageRepository.GetPlasticUsageByIdAsync(id);
        }
        public async Task<List<PlasticUsage>> GetPlasticUsageByUserId(int userId)
        {
          return await _plasticUsageRepository.GetPlasticUsageByUserId(userId);
        }
        public async Task UpdatePlasticUsageAsync(PlasticUsage plasticUsage)
        {
            float totalCO2 = 0;
            foreach (var item in plasticUsage.PlasticItems)
            {
                await _plasticItemRepository.UpdatePlasticItemAsync(item);
                totalCO2 += _categorySelect.PlasticCO2Emission(item) * item.Weight;
            }
            plasticUsage.CO2emission = totalCO2;
            await _plasticUsageRepository.UpdatePlasticUsageAsync(plasticUsage);
        }
    }
}

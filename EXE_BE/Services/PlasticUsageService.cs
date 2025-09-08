using EXE_BE.Data.Repository;
using EXE_BE.Models;
using Microsoft.EntityFrameworkCore;

namespace EXE_BE.Services
{
    public class PlasticUsageService
    {
        private readonly PlasticUsageRepository _plasticUsageRepository;
        
        public PlasticUsageService(PlasticUsageRepository plasticUsageRepository)
        {
            _plasticUsageRepository = plasticUsageRepository;
            
        }
        public async Task<PlasticUsage> AddPlasticUsageAsync(PlasticUsage plasticUsage)
        {
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
    }
}

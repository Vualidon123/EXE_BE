using EXE_BE.Data.Repository;
using EXE_BE.Models;

namespace EXE_BE.Services
{
    public class ChallengeService
    {
        private readonly ChallengeRepository _challengeRepository;
        
        public ChallengeService(ChallengeRepository challengeRepository)
        {
            _challengeRepository = challengeRepository;
            
        }
        public async Task<List<Challenge>> GetAllChallengesAsync()
        {
            return await _challengeRepository.GetAllChallengesAsync();
        }
        public async Task<Challenge?> GetChallengeByIdAsync(int id)
        {
            return await _challengeRepository.GetChallengeByIdAsync(id);
        }
        public async Task CreateChallengeAsync(Challenge challenge)
        {
            await _challengeRepository.CreateChallengeAsync(challenge);
        }
        public async Task UpdateChallengeAsync(Challenge challenge)
        {
            await _challengeRepository.UpdateChallengeAsync(challenge);
        }
        public async Task DeleteChallengeAsync(int id)
        {
            await _challengeRepository.DeleteChallengeAsync(id);
        }
       
    }
}

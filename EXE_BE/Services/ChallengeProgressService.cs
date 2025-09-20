using EXE_BE.Data.Repository;
using EXE_BE.Models;

namespace EXE_BE.Services
{
    public class ChallengeProgressService
    {
        private readonly ChallengeProgressRepository _challengeProgressRepository;
        public ChallengeProgressService(ChallengeProgressRepository challengeProgressRepository)
        {
            _challengeProgressRepository = challengeProgressRepository;
        }
        public async Task<List<ChallengeProgress>> GetAllChallengeProgressAsync()
        {
            return await _challengeProgressRepository.GetAllChallengeProgressAsync();
        }
        public async Task<ChallengeProgress?> GetChallengeProgressByIdAsync(int id)
        {
            return await _challengeProgressRepository.GetChallengeProgressByIdAsync(id);
        }
        public async Task CreateChallengeProgressAsync(ChallengeProgress challengeProgress)
        {
            await _challengeProgressRepository.CreateChallengeProgressAsync(challengeProgress);
        }
         
    }
}

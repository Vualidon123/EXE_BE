using EXE_BE.Models;
using Microsoft.EntityFrameworkCore;

namespace EXE_BE.Data.Repository
{
    public class ChallengeProgressRepository
    {
        private readonly AppDbContext _context;
        public ChallengeProgressRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<ChallengeProgress>> GetAllChallengeProgressAsync()
        {
            return await _context.ChallengeProgresses.ToListAsync();
        }
        public async Task<ChallengeProgress?> GetChallengeProgressByIdAsync(int id)
        {
            return await _context.ChallengeProgresses.FindAsync(id);
        }
        public async Task CreateChallengeProgressAsync(ChallengeProgress challengeProgress)
        {
            _context.ChallengeProgresses.Add(challengeProgress);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateChallengeProgressAsync(ChallengeProgress challengeProgress)
        {
            _context.ChallengeProgresses.Update(challengeProgress);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteChallengeProgressAsync(int id)
        {
            var challengeProgress = await _context.ChallengeProgresses.FindAsync(id);
            if (challengeProgress != null)
            {
                _context.ChallengeProgresses.Remove(challengeProgress);
                await _context.SaveChangesAsync();
            }
        }
    }
}

using EXE_BE.Models;
using Microsoft.EntityFrameworkCore;

namespace EXE_BE.Data.Repository
{
    public class ChallengeRepository
    {
        private readonly AppDbContext _context;
        public ChallengeRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<List<Challenge>> GetAllChallengesAsync()
        {
            return await _context.Challenges
            
                .ToListAsync();
        }
        public async Task<Challenge?> GetChallengeByIdAsync(int id)
        {
            return await _context.Challenges.FindAsync(id);
        }
        public async Task CreateChallengeAsync(Challenge challenge)
        {
            _context.Challenges.Add(challenge);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateChallengeAsync(Challenge challenge)
        {
            _context.Challenges.Update(challenge);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteChallengeAsync(int id)
        {
            var challenge = await _context.Challenges.FindAsync(id);
            if (challenge != null)
            {
                _context.Challenges.Remove(challenge);
                await _context.SaveChangesAsync();
            }
        }

    }
}

using EXE_BE.Models;
using EXE_BE.Controllers.ViewModel;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EXE_BE.Data.Repository
{
    public class TransactionRepository
    {
        private readonly AppDbContext _context;

        public TransactionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Transaction> AddTransactionAsync(Transaction transaction)
        {
            transaction.CreatedAt = DateTime.UtcNow;
            _context.Add(transaction);
            await _context.SaveChangesAsync();
            return transaction;
        }

        public async Task<Transaction?> GetTransactionByIdAsync(int id)
        {
            return await _context.Set<Transaction>()
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<List<Transaction>> GetTransactionsByUserIdAsync(int userId)
        {
            return await _context.Set<Transaction>()
                .Include(t => t.User)
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task UpdateTransactionAsync(Transaction transaction)
        {
            transaction.UpdatedAt = DateTime.UtcNow;
            _context.Update(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteTransactionAsync(int id)
        {
            var transaction = await _context.Set<Transaction>().FindAsync(id);
            if (transaction != null)
            {
                _context.Remove(transaction);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<Transaction>> GetAllTransactionsAsync()
        {
            return await _context.Set<Transaction>()
                .Include(t => t.User)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<(List<Transaction> transactions, int totalCount)> GetFilteredTransactionsAsync(TransactionFilterRequest filter)
        {
            var query = _context.Set<Transaction>()
                .Include(t => t.User)
                .AsQueryable();

            // Apply filters
            if (filter.UserId.HasValue)
                query = query.Where(t => t.UserId == filter.UserId.Value);

            if (filter.Status.HasValue)
                query = query.Where(t => t.Status == filter.Status.Value);

            if (filter.MinAmount.HasValue)
                query = query.Where(t => t.Amount >= filter.MinAmount.Value);

            if (filter.MaxAmount.HasValue)
                query = query.Where(t => t.Amount <= filter.MaxAmount.Value);

            if (filter.StartDate.HasValue)
                query = query.Where(t => t.CreatedAt >= filter.StartDate.Value);

            if (filter.EndDate.HasValue)
                query = query.Where(t => t.CreatedAt <= filter.EndDate.Value);

            var totalCount = await query.CountAsync();

            var transactions = await query
                .OrderByDescending(t => t.CreatedAt)
                .Skip((filter.Page - 1) * filter.PageSize)
                .Take(filter.PageSize)
                .ToListAsync();

            return (transactions, totalCount);
        }
    }
}
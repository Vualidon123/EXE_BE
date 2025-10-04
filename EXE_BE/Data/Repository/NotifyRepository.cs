using EXE_BE.Models;
using Microsoft.EntityFrameworkCore;

namespace EXE_BE.Data.Repository
{
    public class NotifyRepository
    {
        private readonly AppDbContext _context;

        public NotifyRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Notify?> GetByIdAsync(int id)
        {
            return await _context.Notifies.FindAsync(id);
        }

        public async Task<List<Notify>> GetAllAsync()
        {
            return await _context.Notifies
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Notify>> GetActiveNotificationsAsync()
        {
            return await _context.Notifies
                .Where(n => n.IsActive)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<List<Notify>> GetByReasonAsync(NotifyReason reason)
        {
            return await _context.Notifies
                .Where(n => n.Reason == reason && n.IsActive)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<Notify> CreateAsync(Notify notify)
        {
            _context.Notifies.Add(notify);
            await _context.SaveChangesAsync();
            return notify;
        }

        public async Task<Notify?> UpdateAsync(Notify notify)
        {
            var existingNotify = await _context.Notifies.FindAsync(notify.Id);
            if (existingNotify == null)
            {
                return null;
            }

            existingNotify.Title = notify.Title;
            existingNotify.Content = notify.Content;
            existingNotify.Reason = notify.Reason;
            existingNotify.UpdatedAt = DateTime.UtcNow;
            existingNotify.IsActive = notify.IsActive;

            await _context.SaveChangesAsync();
            return existingNotify;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var notify = await _context.Notifies.FindAsync(id);
            if (notify == null)
            {
                return false;
            }

            _context.Notifies.Remove(notify);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            var notify = await _context.Notifies.FindAsync(id);
            if (notify == null)
            {
                return false;
            }

            notify.IsActive = false;
            notify.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<List<Notify>> GetPagedAsync(int page, int pageSize)
        {
            return await _context.Notifies
                .OrderByDescending(n => n.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        public async Task<int> GetTotalCountAsync()
        {
            return await _context.Notifies.CountAsync();
        }
    }
}
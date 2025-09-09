using EXE_BE.Models.ItemList;

namespace EXE_BE.Data.Repository
{
    public class PlasticItemRepository
    {
        private readonly AppDbContext _context;
        public PlasticItemRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<PlasticItem> AddPlasticItemAsync(PlasticItem plasticItem)
        {
            _context.PlasticItems.Add(plasticItem);
            await _context.SaveChangesAsync();
            return plasticItem;
        }
        public async Task<PlasticItem?> GetPlasticItemByIdAsync(int id)
        {
            return await _context.PlasticItems.FindAsync(id);
        }
        public async Task UpdatePlasticItemAsync(PlasticItem plasticItem)
        {
            _context.PlasticItems.Update(plasticItem);
            await _context.SaveChangesAsync();
        }
    }
}

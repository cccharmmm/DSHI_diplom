using DSHI_diplom.Model;
using Microsoft.EntityFrameworkCore;
using DSHI_diplom.Services.Interfaces;


namespace DSHI_diplom.Services.Implementations
{
    public class CollectionOfThMlWithThService : ICollectionOfThMlWithThService
    {
        private readonly DiplomContext _context;
        public CollectionOfThMlWithThService(DiplomContext context)
        {
            _context = context;
        }

        public async Task<List<CollectionOfThMlWithThM>> GetAllAsync()
        {
            return await _context.CollectionOfThMlWithThMs.ToListAsync();
        }

        public async Task<CollectionOfThMlWithThM?> GetByIdAsync(int id)
        {
            return await _context.CollectionOfThMlWithThMs.FindAsync(id);
        }

        public async Task AddAsync(CollectionOfThMlWithThM collectionOfThMlWithThM)
        {
            _context.CollectionOfThMlWithThMs.Add(collectionOfThMlWithThM);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(CollectionOfThMlWithThM collectionOfThMlWithThM)
        {
            _context.CollectionOfThMlWithThMs.Update(collectionOfThMlWithThM);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var collectionOfThMlWithThM = await _context.CollectionOfThMlWithThMs.FindAsync(id);
            if (collectionOfThMlWithThM != null)
            {
                _context.CollectionOfThMlWithThMs.Remove(collectionOfThMlWithThM);
                await _context.SaveChangesAsync();
            }
        }
    }
}

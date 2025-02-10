using DSHI_diplom.Model;
using Microsoft.EntityFrameworkCore;
using DSHI_diplom.Services.Interfaces;

namespace DSHI_diplom.Services.Implementations
{
    public class CollectionOfTheoreticalMaterialService : ICollectionOfTheoreticalMaterialService
    {
        private readonly DiplomContext _context;
        public CollectionOfTheoreticalMaterialService(DiplomContext context)
        {
            _context = context;
        }

        public async Task<List<CollectionOfTheoreticalMaterial>> GetAllAsync()
        {
            return await _context.CollectionOfTheoreticalMaterials.ToListAsync();
        }

        public async Task<CollectionOfTheoreticalMaterial?> GetByIdAsync(int id)
        {
            return await _context.CollectionOfTheoreticalMaterials.FindAsync(id);
        }

        public async Task AddAsync(CollectionOfTheoreticalMaterial collectionOfTheoreticalMaterial)
        {
            _context.CollectionOfTheoreticalMaterials.Add(collectionOfTheoreticalMaterial);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(CollectionOfTheoreticalMaterial collectionOfTheoreticalMaterial)
        {
            _context.CollectionOfTheoreticalMaterials.Update(collectionOfTheoreticalMaterial);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var collectionOfTheoreticalMaterial = await _context.CollectionOfTheoreticalMaterials.FindAsync(id);
            if (collectionOfTheoreticalMaterial != null)
            {
                _context.CollectionOfTheoreticalMaterials.Remove(collectionOfTheoreticalMaterial);
                await _context.SaveChangesAsync();
            }
        }
    }
}

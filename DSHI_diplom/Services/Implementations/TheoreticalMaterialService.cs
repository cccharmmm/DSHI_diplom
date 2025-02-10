using DSHI_diplom.Model;
using Microsoft.EntityFrameworkCore;
using DSHI_diplom.Services.Interfaces;

namespace DSHI_diplom.Services.Implementations
{
    public class TheoreticalMaterialService : ITheoreticalMaterialService
    {
        private readonly DiplomContext _context;
        public TheoreticalMaterialService(DiplomContext context)
        {
            _context = context;
        }

        public async Task<List<TheoreticalMaterial>> GetAllAsync()
        {
            return await _context.TheoreticalMaterials.ToListAsync();
        }

        public async Task<TheoreticalMaterial?> GetByIdAsync(int id)
        {
            return await _context.TheoreticalMaterials.FindAsync(id);
        }

        public async Task AddAsync(TheoreticalMaterial theoreticalMaterial)
        {
            _context.TheoreticalMaterials.Add(theoreticalMaterial);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TheoreticalMaterial theoreticalMaterial)
        {
            _context.TheoreticalMaterials.Update(theoreticalMaterial);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var theoreticalMaterial = await _context.TheoreticalMaterials.FindAsync(id);
            if (theoreticalMaterial != null)
            {
                _context.TheoreticalMaterials.Remove(theoreticalMaterial);
                await _context.SaveChangesAsync();
            }
        }
    }
}

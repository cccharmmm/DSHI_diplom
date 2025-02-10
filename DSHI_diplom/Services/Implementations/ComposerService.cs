using DSHI_diplom.Model;
using Microsoft.EntityFrameworkCore;
using DSHI_diplom.Services.Interfaces;


namespace DSHI_diplom.Services.Implementations
{
    public class ComposerService : IComposerService
    {
        private readonly DiplomContext _context;
        public ComposerService(DiplomContext context)
        {
            _context = context;
        }

        public async Task<List<Composer>> GetAllAsync()
        {
            return await _context.Composers.ToListAsync();
        }

        public async Task<Composer?> GetByIdAsync(int id)
        {
            return await _context.Composers.FindAsync(id);
        }

        public async Task AddAsync(Composer composer)
        {
            _context.Composers.Add(composer);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Composer composer)
        {
            _context.Composers.Update(composer);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var composer = await _context.Composers.FindAsync(id);
            if (composer != null)
            {
                _context.Composers.Remove(composer);
                await _context.SaveChangesAsync();
            }
        }
    }
}

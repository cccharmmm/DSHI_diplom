using DSHI_diplom.Model;
using Microsoft.EntityFrameworkCore;
using DSHI_diplom.Services.Interfaces;

namespace DSHI_diplom.Services.Implementations
{
    public class MusicalFormService : IMusicalFormService
    {
        private readonly DiplomContext _context;
        public MusicalFormService(DiplomContext context)
        {
            _context = context;
        }

        public async Task<List<MusicalForm>> GetAllAsync()
        {
            return await _context.MusicalForms.ToListAsync();
        }

        public async Task<MusicalForm?> GetByIdAsync(int id)
        {
            return await _context.MusicalForms.FindAsync(id);
        }

        public async Task AddAsync(MusicalForm musicalForm)
        {
            _context.MusicalForms.Add(musicalForm);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(MusicalForm musicalForm)
        {
            _context.MusicalForms.Update(musicalForm);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var musicalForm = await _context.MusicalForms.FindAsync(id);
            if (musicalForm != null)
            {
                _context.MusicalForms.Remove(musicalForm);
                await _context.SaveChangesAsync();
            }
        }
    }
}

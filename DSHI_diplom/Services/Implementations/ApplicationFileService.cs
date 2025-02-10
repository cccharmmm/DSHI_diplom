using DSHI_diplom.Model;
using Microsoft.EntityFrameworkCore;
using DSHI_diplom.Services.Interfaces;

namespace DSHI_diplom.Services.Implementations
{
    public class ApplicationFileService : IApplicationFileService
    {
        private readonly DiplomContext _context;
        public ApplicationFileService(DiplomContext context)
        {
            _context = context;
        }

        public async Task<List<ApplicationFile>> GetAllAsync()
        {
            return await _context.ApplicationFiles.ToListAsync();
        }

        public async Task<ApplicationFile?> GetByIdAsync(int id)
        {
            return await _context.ApplicationFiles.FindAsync(id);
        }

        public async Task AddAsync(ApplicationFile applicationFile)
        {
            _context.ApplicationFiles.Add(applicationFile);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(ApplicationFile applicationFile)
        {
            _context.ApplicationFiles.Update(applicationFile);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var applicationFile = await _context.ApplicationFiles.FindAsync(id);
            if (applicationFile != null)
            {
                _context.ApplicationFiles.Remove(applicationFile);
                await _context.SaveChangesAsync();
            }
        }
    }
}

using DSHI_diplom.Model;
using Microsoft.EntityFrameworkCore;
using DSHI_diplom.Services.Interfaces;

namespace DSHI_diplom.Services.Implementations
{
    public class ClassService : IClassService
    {
        private readonly DiplomContext _context;
        public ClassService(DiplomContext context)
        {
            _context = context;
        }

        public async Task<List<Class>> GetAllAsync()
        {
            return await _context.Classes.ToListAsync();
        }

        public async Task<Class?> GetByIdAsync(int id)
        {
            return await _context.Classes.FindAsync(id);
        }

        public async Task AddAsync(Class class_)
        {
            _context.Classes.Add(class_);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Class class_)
        {
            _context.Classes.Update(class_);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var class_ = await _context.Classes.FindAsync(id);
            if (class_ != null)
            {
                _context.Classes.Remove(class_);
                await _context.SaveChangesAsync();
            }
        }
    }
}

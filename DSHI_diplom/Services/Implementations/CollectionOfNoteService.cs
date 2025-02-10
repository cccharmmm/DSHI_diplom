using DSHI_diplom.Model;
using Microsoft.EntityFrameworkCore;
using DSHI_diplom.Services.Interfaces;

namespace DSHI_diplom.Services.Implementations
{
    public class CollectionOfNoteService : ICollectionOfNoteService
    {
        private readonly DiplomContext _context;
        public CollectionOfNoteService(DiplomContext context)
        {
            _context = context;
        }

        public async Task<List<CollectionOfNote>> GetAllAsync()
        {
            return await _context.CollectionOfNotes.ToListAsync();
        }

        public async Task<CollectionOfNote?> GetByIdAsync(int id)
        {
            return await _context.CollectionOfNotes.FindAsync(id);
        }

        public async Task AddAsync(CollectionOfNote collectionOfNote)
        {
            _context.CollectionOfNotes.Add(collectionOfNote);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(CollectionOfNote collectionOfNote)
        {
            _context.CollectionOfNotes.Update(collectionOfNote);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var collectionOfNote = await _context.CollectionOfNotes.FindAsync(id);
            if (collectionOfNote != null)
            {
                _context.CollectionOfNotes.Remove(collectionOfNote);
                await _context.SaveChangesAsync();
            }
        }
    }
}

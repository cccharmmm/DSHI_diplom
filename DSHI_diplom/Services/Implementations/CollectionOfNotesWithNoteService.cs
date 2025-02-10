using DSHI_diplom.Model;
using Microsoft.EntityFrameworkCore;
using DSHI_diplom.Services.Interfaces;


namespace DSHI_diplom.Services.Implementations
{
    public class CollectionOfNotesWithNoteService : ICollectionOfNotesWithNoteService
    {
        private readonly DiplomContext _context;
        public CollectionOfNotesWithNoteService(DiplomContext context)
        {
            _context = context;
        }

        public async Task<List<CollectionOfNotesWithNote>> GetAllAsync()
        {
            return await _context.CollectionOfNotesWithNotes.ToListAsync();
        }

        public async Task<CollectionOfNotesWithNote?> GetByIdAsync(int id)
        {
            return await _context.CollectionOfNotesWithNotes.FindAsync(id);
        }

        public async Task AddAsync(CollectionOfNotesWithNote collectionOfNoteWithNote)
        {
            _context.CollectionOfNotesWithNotes.Add(collectionOfNoteWithNote);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(CollectionOfNotesWithNote collectionOfNoteWithNote)
        {
            _context.CollectionOfNotesWithNotes.Update(collectionOfNoteWithNote);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var collectionOfNoteWithNote = await _context.CollectionOfNotesWithNotes.FindAsync(id);
            if (collectionOfNoteWithNote != null)
            {
                _context.CollectionOfNotesWithNotes.Remove(collectionOfNoteWithNote);
                await _context.SaveChangesAsync();
            }
        }
    }
}

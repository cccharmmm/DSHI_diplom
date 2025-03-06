using DSHI_diplom.Model;
using Microsoft.EntityFrameworkCore;
using DSHI_diplom.Services.Interfaces;
using System.Net.Http;
using Microsoft.AspNetCore.Components;

namespace DSHI_diplom.Services.Implementations
{
    public class NoteService : INoteService
    {
        private readonly DiplomContext _context;
        public NoteService(DiplomContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
      
        public async Task<List<Note>> GetAllAsync()
        {
            return await _context.Notes
                .Include(n => n.File)
                 .Include(n => n.AudioFile)
                .ToListAsync();
        }
        public async Task<List<Note>> GetFilteredNotesBySearchAsync(string searchText)
        {
            var notes = await _context.Notes
                .Where(n => n.Name.Contains(searchText) || (n.Composer != null && n.Composer.Name.Contains(searchText)))
                .ToListAsync();
            return notes;
        }


        public async Task<Note?> GetByIdAsync(int id)
        {
            return await _context.Notes
                .Include(n => n.Composer)
                .Include(n => n.Class)
                .Include(n => n.Musicalform)
                .Include(n => n.Instrument)
                .FirstOrDefaultAsync(n => n.Id == id);
        }

        public async Task AddAsync(Note note)
        {
            _context.Notes.Add(note);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Note note)
        {
            _context.Notes.Update(note);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var note = await _context.Notes.FindAsync(id);
            if (note != null)
            {
                _context.Notes.Remove(note);
                await _context.SaveChangesAsync();
            }
        }
    }
}

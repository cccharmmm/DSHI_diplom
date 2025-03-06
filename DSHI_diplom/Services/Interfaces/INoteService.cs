using DSHI_diplom.Model;

namespace DSHI_diplom.Services.Interfaces
{
    public interface INoteService
    {
        Task<List<Note>> GetAllAsync();
        Task<Note?> GetByIdAsync(int id);
        Task AddAsync(Note note);
        Task UpdateAsync(Note note);
        Task DeleteAsync(int id);
        Task<List<Note>> GetFilteredNotesBySearchAsync(string searchText);
    }
}

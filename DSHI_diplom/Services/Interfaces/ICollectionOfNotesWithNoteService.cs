using DSHI_diplom.Model;

namespace DSHI_diplom.Services.Interfaces
{
    public interface ICollectionOfNotesWithNoteService
    {
        Task<List<CollectionOfNotesWithNote>> GetAllAsync();
        Task<CollectionOfNotesWithNote?> GetByIdAsync(int id);
        Task AddAsync(CollectionOfNotesWithNote collectionOfNotesWithNote);
        Task UpdateAsync(CollectionOfNotesWithNote collectionOfNotesWithNote);
        Task DeleteAsync(int id);

    }
}

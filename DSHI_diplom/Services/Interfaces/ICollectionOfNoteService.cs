using DSHI_diplom.Model;

namespace DSHI_diplom.Services.Interfaces
{
    public interface ICollectionOfNoteService
    {
        Task<List<CollectionOfNote>> GetAllAsync();
        Task<CollectionOfNote?> GetByIdAsync(int id);
        Task AddAsync(CollectionOfNote collectionOfNote);
        Task UpdateAsync(CollectionOfNote collectionOfNote);
        Task DeleteAsync(int id);

    }
}

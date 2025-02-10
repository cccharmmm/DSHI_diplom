using DSHI_diplom.Model;

namespace DSHI_diplom.Services.Interfaces
{
    public interface ICollectionOfThMlWithThService
    {
        Task<List<CollectionOfThMlWithThM>> GetAllAsync();
        Task<CollectionOfThMlWithThM?> GetByIdAsync(int id);
        Task AddAsync(CollectionOfThMlWithThM collectionOfThMlWithThM);
        Task UpdateAsync(CollectionOfThMlWithThM collectionOfThMlWithThM);
        Task DeleteAsync(int id);

    }
}

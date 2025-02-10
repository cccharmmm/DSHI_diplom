using DSHI_diplom.Model;

namespace DSHI_diplom.Services.Interfaces
{
    public interface ICollectionOfTheoreticalMaterialService
    {
        Task<List<CollectionOfTheoreticalMaterial>> GetAllAsync();
        Task<CollectionOfTheoreticalMaterial?> GetByIdAsync(int id);
        Task AddAsync(CollectionOfTheoreticalMaterial collectionOfTheoreticalMaterial);
        Task UpdateAsync(CollectionOfTheoreticalMaterial collectionOfTheoreticalMaterial);
        Task DeleteAsync(int id);

    }
}

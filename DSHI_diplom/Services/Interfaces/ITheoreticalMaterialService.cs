using DSHI_diplom.Model;

namespace DSHI_diplom.Services.Interfaces
{
    public interface ITheoreticalMaterialService
    {
        Task<List<TheoreticalMaterial>> GetAllAsync();
        Task<TheoreticalMaterial?> GetByIdAsync(int id);
        Task AddAsync(TheoreticalMaterial theoreticalMaterial);
        Task UpdateAsync(TheoreticalMaterial theoreticalMaterial);
        Task DeleteAsync(int id);

        Task<List<TheoreticalMaterial>> GetFilteredTheoryAsync(string author, string subject, string class_);

        Task<List<TheoreticalMaterial>> GetFilteredTheoryBySearchAsync(string searchText);
    }
}

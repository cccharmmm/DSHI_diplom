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

    }
}

using DSHI_diplom.Model;

namespace DSHI_diplom.Services.Interfaces
{
    public interface IApplicationFileService
    {
        Task<List<ApplicationFile>> GetAllAsync();
        Task<ApplicationFile?> GetByIdAsync(int id);
        Task AddAsync(ApplicationFile applicationFile);
        Task UpdateAsync(ApplicationFile applicationFile);
        Task DeleteAsync(int id);

    }
}

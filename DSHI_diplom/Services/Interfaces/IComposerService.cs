using DSHI_diplom.Model;

namespace DSHI_diplom.Services.Interfaces
{
    public interface IComposerService
    {
        Task<List<Composer>> GetAllAsync();
        Task<Composer?> GetByIdAsync(int id);
        Task AddAsync(Composer composer);
        Task UpdateAsync(Composer composer);
        Task DeleteAsync(int id);

    }
}

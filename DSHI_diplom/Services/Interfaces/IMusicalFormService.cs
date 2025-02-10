using DSHI_diplom.Model;

namespace DSHI_diplom.Services.Interfaces
{
    public interface IMusicalFormService
    {
        Task<List<MusicalForm>> GetAllAsync();
        Task<MusicalForm?> GetByIdAsync(int id);
        Task AddAsync(MusicalForm musicalForm);
        Task UpdateAsync(MusicalForm musicalForm);
        Task DeleteAsync(int id);

    }
}

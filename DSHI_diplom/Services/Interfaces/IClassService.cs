using DSHI_diplom.Model;

namespace DSHI_diplom.Services.Interfaces
{
    public interface IClassService
    {
        Task<List<Class>> GetAllAsync();
        Task<Class?> GetByIdAsync(int id);
        Task AddAsync(Class class_);
        Task UpdateAsync(Class class_);
        Task DeleteAsync(int id);

    }
}

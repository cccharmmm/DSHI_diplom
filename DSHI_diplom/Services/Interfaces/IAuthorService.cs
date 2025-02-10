using DSHI_diplom.Model;

namespace DSHI_diplom.Services.Interfaces
{
    public interface IAuthorService
    {
        Task<List<Author>> GetAllAsync();
        Task<Author?> GetByIdAsync(int id);
        Task AddAsync(Author author);
        Task UpdateAsync(Author author);
        Task DeleteAsync(int id);

    }
}

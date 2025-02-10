using DSHI_diplom.Model;

namespace DSHI_diplom.Services.Interfaces
{
    public interface IAnswerService
    {
        Task<List<Answer>> GetAllAsync();
        Task<Answer?> GetByIdAsync(int id);
        Task AddAsync(Answer answer);
        Task UpdateAsync(Answer answer);
        Task DeleteAsync(int id);

    }
}

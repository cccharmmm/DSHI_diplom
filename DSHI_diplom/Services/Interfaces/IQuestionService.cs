using DSHI_diplom.Model;

namespace DSHI_diplom.Services.Interfaces
{
    public interface IQuestionService
    {
        Task<List<Question>> GetAllAsync();
        Task<Question?> GetByIdAsync(int id);
        Task AddAsync(Question question);
        Task UpdateAsync(Question question);
        Task DeleteAsync(int id);

    }
}

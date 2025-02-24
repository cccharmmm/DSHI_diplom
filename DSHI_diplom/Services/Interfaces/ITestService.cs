using DSHI_diplom.Model;

namespace DSHI_diplom.Services.Interfaces
{
    public interface ITestService
    {
        Task<List<Test>> GetAllAsync();
        Task<Test?> GetByIdAsync(int id);
        Task AddAsync(Test test);
        Task UpdateAsync(Test test);
        Task DeleteAsync(int id);
        Task<List<Test>> GetFilteredTestBySearchAsync(string searchText);
        Task<List<Test>> GetFilteredTestAsync(string subject, string class_);

    }
}

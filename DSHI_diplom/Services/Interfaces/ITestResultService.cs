using DSHI_diplom.Model;

namespace DSHI_diplom.Services.Interfaces
{
    public interface ITestResultService
    {
        Task<List<TestResult>> GetAllAsync();
        Task<TestResult?> GetByIdAsync(int id);
        Task AddAsync(TestResult testResult);
        Task UpdateAsync(TestResult testResult);
        Task DeleteAsync(int id);

    }
}

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
        Task<TestResult?> GetLatestResultByUserIdAsync(int userId);
        Task<(List<Answer> Answers, int QuestionCount)> GetAnswersByTestResultIdAsync(int testResultId);
        Task<int> GetQuestionCountByTestIdAsync(int testId);
        Task<int> GetAttemptNumberAsync(int testId, int userId);
        Task<TestResult> GetTestResultByIdAsync(int testResultId);

    }
}

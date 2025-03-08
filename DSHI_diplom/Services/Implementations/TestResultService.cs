using DSHI_diplom.Model;
using Microsoft.EntityFrameworkCore;
using DSHI_diplom.Services.Interfaces;

namespace DSHI_diplom.Services.Implementations
{
    public class TestResultService : ITestResultService
    {
        private readonly DiplomContext _context;
        public TestResultService(DiplomContext context)
        {
            _context = context;
        }
        public async Task<TestResult> GetTestResultByIdAsync(int testResultId)
        {
            var testResult = await _context.TestResults
                .Include(tr => tr.Test) 
                    .ThenInclude(t => t.Questions) 
                        .ThenInclude(q => q.Answers) 
                .Include(tr => tr.User)
                .FirstOrDefaultAsync(tr => tr.Id == testResultId);

            if (testResult == null)
            {
                throw new ArgumentException("TestResult не найден.");
            }

            return testResult;
        }
        public async Task<int> GetAttemptNumberAsync(int testId, int userId)
        {
            return await _context.TestResults
                .Where(tr => tr.TestId == testId && tr.UserId == userId)
                .CountAsync();
        }
        public async Task<TestResult?> GetLatestResultByUserIdAsync(int userId)
        {
            var results = await _context.TestResults
                .Include(tr => tr.Test)
                .ThenInclude(t => t.Subject)
                .Include(tr => tr.Test)
                .ThenInclude(t => t.Class)
                .Where(tr => tr.UserId == userId)
                .OrderByDescending(tr => tr.Date) 
                .ThenByDescending(tr => tr.Id)   
                .ToListAsync();

            Console.WriteLine("Все результаты теста для пользователя:");
            foreach (var result in results)
            {
                Console.WriteLine($"ID: {result.Id}, CorrectAnswers: {result.CorrectAnswers}, Date: {result.Date}");
            }

            var latestResult = results.FirstOrDefault();
            Console.WriteLine($"Последний результат теста для пользователя {userId}: {latestResult?.Id}, Правильных ответов: {latestResult?.CorrectAnswers}, Дата: {latestResult?.Date}");

            return latestResult;
        }
        public async Task<int> GetQuestionCountByTestIdAsync(int testId)
        {
            var count = await _context.Questions
                .Where(q => q.TestId == testId)
                .CountAsync();

            Console.WriteLine($"Количество вопросов в тесте {testId}: {count}");

            return count;
        }

        public async Task<(List<Answer> Answers, int QuestionCount)> GetAnswersByTestResultIdAsync(int testResultId)
        {
            var testResult = await _context.TestResults
                .Include(tr => tr.Test) 
                .FirstOrDefaultAsync(tr => tr.Id == testResultId);

            if (testResult == null)
            {
                throw new ArgumentException("TestResult не найден.");
            }

            var testId = testResult.TestId;

            var questions = await _context.Questions
                .Where(q => q.TestId == testId)
                .ToListAsync();

            var answers = await _context.Answers
                .Where(a => questions.Select(q => q.Id).Contains(a.QuestionId))
                .ToListAsync();

            Console.WriteLine("Ответы:");
            foreach (var answer in answers)
            {
                Console.WriteLine($"ID: {answer.Id}, QuestionId: {answer.QuestionId}, Rightt: {answer.Rightt}");
            }

            var questionCount = questions.Count;

            return (answers, questionCount);
        }
        public async Task<List<TestResult>> GetAllAsync()
        {
            return await _context.TestResults.ToListAsync();
        }

        public async Task<TestResult?> GetByIdAsync(int id)
        {
            return await _context.TestResults.FindAsync(id);
        }

        public async Task AddAsync(TestResult testResult)
        {
            _context.TestResults.Add(testResult);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(TestResult testResult)
        {
            _context.TestResults.Update(testResult);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var testResult = await _context.TestResults.FindAsync(id);
            if (testResult != null)
            {
                _context.TestResults.Remove(testResult);
                await _context.SaveChangesAsync();
            }
        }
    }
}

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

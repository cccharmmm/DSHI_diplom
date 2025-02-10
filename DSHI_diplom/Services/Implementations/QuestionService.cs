using DSHI_diplom.Model;
using Microsoft.EntityFrameworkCore;
using DSHI_diplom.Services.Interfaces;

namespace DSHI_diplom.Services.Implementations
{
    public class QuestionService : IQuestionService
    {
        private readonly DiplomContext _context;
        public QuestionService(DiplomContext context)
        {
            _context = context;
        }

        public async Task<List<Question>> GetAllAsync()
        {
            return await _context.Questions.ToListAsync();
        }

        public async Task<Question?> GetByIdAsync(int id)
        {
            return await _context.Questions.FindAsync(id);
        }

        public async Task AddAsync(Question question)
        {
            _context.Questions.Add(question);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Question question)
        {
            _context.Questions.Update(question);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var question = await _context.Questions.FindAsync(id);
            if (question != null)
            {
                _context.Questions.Remove(question);
                await _context.SaveChangesAsync();
            }
        }
    }
}

using DSHI_diplom.Model;
using Microsoft.EntityFrameworkCore;
using DSHI_diplom.Services.Interfaces;


namespace DSHI_diplom.Services.Implementations
{
    public class AnswerService : IAnswerService
    {
        private readonly DiplomContext _context;
        public AnswerService(DiplomContext context)
        {
            _context = context;
        }

        public async Task<List<Answer>> GetAllAsync()
        {
            return await _context.Answers.ToListAsync();
        }

        public async Task<Answer?> GetByIdAsync(int id)
        {
            return await _context.Answers.FindAsync(id);
        }

        public async Task AddAsync(Answer answer)
        {
            _context.Answers.Add(answer);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Answer answer)
        {
            _context.Answers.Update(answer);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var answer = await _context.Answers.FindAsync(id);
            if (answer != null)
            {
                _context.Answers.Remove(answer);
                await _context.SaveChangesAsync();
            }
        }
    }
}

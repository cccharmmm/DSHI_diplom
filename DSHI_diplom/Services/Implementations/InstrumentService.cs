using DSHI_diplom.Model;
using Microsoft.EntityFrameworkCore;
using DSHI_diplom.Services.Interfaces;

namespace DSHI_diplom.Services.Implementations
{
    public class InstrumentService : IInstrumentService
    {
        private readonly DiplomContext _context;
        public InstrumentService(DiplomContext context)
        {
            _context = context;
        }

        public async Task<List<Instrument>> GetAllAsync()
        {
            return await _context.Instruments.ToListAsync();
        }

        public async Task<Instrument?> GetByIdAsync(int id)
        {
            return await _context.Instruments.FindAsync(id);
        }

        public async Task AddAsync(Instrument instrument)
        {
            _context.Instruments.Add(instrument);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Instrument instrument)
        {
            _context.Instruments.Update(instrument);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var instrument = await _context.Instruments.FindAsync(id);
            if (instrument != null)
            {
                _context.Instruments.Remove(instrument);
                await _context.SaveChangesAsync();
            }
        }
    }
}

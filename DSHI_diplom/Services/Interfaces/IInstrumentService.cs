using DSHI_diplom.Model;

namespace DSHI_diplom.Services.Interfaces
{
    public interface IInstrumentService
    {
        Task<List<Instrument>> GetAllAsync();
        Task<Instrument?> GetByIdAsync(int id);
        Task AddAsync(Instrument instrument);
        Task UpdateAsync(Instrument instrument);
        Task DeleteAsync(int id);

    }
}

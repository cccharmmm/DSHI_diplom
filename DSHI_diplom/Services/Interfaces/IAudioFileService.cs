using DSHI_diplom.Model;

namespace DSHI_diplom.Services.Interfaces
{
    public interface IAudioFileService
    {
        Task<List<AudioFile>> GetAllAsync();
        Task<AudioFile?> GetByIdAsync(int id);
        Task AddAsync(AudioFile audioFile);
        Task UpdateAsync(AudioFile audioFile);
        Task DeleteAsync(int id);

    }
}

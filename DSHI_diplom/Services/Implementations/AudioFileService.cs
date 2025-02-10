using DSHI_diplom.Model;
using Microsoft.EntityFrameworkCore;
using DSHI_diplom.Services.Interfaces;

namespace DSHI_diplom.Services.Implementations
{
    public class AudioFileService : IAudioFileService
    {
        private readonly DiplomContext _context;
        public AudioFileService(DiplomContext context)
        {
            _context = context;
        }

        public async Task<List<AudioFile>> GetAllAsync()
        {
            return await _context.AudioFiles.ToListAsync();
        }

        public async Task<AudioFile?> GetByIdAsync(int id)
        {
            return await _context.AudioFiles.FindAsync(id);
        }

        public async Task AddAsync(AudioFile audioFile)
        {
            _context.AudioFiles.Add(audioFile);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(AudioFile audioFile)
        {
            _context.AudioFiles.Update(audioFile);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var audioFile = await _context.AudioFiles.FindAsync(id);
            if (audioFile != null)
            {
                _context.AudioFiles.Remove(audioFile);
                await _context.SaveChangesAsync();
            }
        }
    }
}

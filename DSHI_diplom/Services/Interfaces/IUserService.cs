using DSHI_diplom.Model;

namespace DSHI_diplom.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<User>> GetAllAsync();
        Task<User?> GetByIdAsync(int id);
        Task AddAsync(User user);
        Task UpdateAsync(User user);
        Task DeleteAsync(int id);
        public Task<User?> AuthenticateAsync(string login, string password);

    }
}

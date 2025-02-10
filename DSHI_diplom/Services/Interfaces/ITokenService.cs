using DSHI_diplom.Model;

namespace DSHI_diplom.Services.Interfaces
{
    public interface ITokenService
    {
        string GenerateToken(User user);
    }
}

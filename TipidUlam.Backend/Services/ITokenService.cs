using TipidUlam.Backend.Models;

namespace TipidUlam.Backend.Services
{
    public interface ITokenService
    {
        (string Token, DateTime ExpiresAt) CreateToken(User user);
    }
}

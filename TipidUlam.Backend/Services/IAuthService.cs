using System.Threading.Tasks;
using TipidUlam.Backend.DTOs;

namespace TipidUlam.Backend.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request);
        Task<AuthResponseDto> LoginAsync(LoginRequestDto request);
        Task<UserProfileDto?> GetProfileAsync(int userId);
    }
}

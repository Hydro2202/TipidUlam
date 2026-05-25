using System;
using System.Threading.Tasks;
using TipidUlam.Backend.DTOs;
using TipidUlam.Backend.Models;
using TipidUlam.Backend.Repositories;

namespace TipidUlam.Backend.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;
        private readonly ITokenService _tokenService;

        public AuthService(IUserRepository userRepository, ITokenService tokenService)
        {
            _userRepository = userRepository;
            _tokenService = tokenService;
        }

        public async Task<AuthResponseDto> RegisterAsync(RegisterRequestDto request)
        {
            if (await _userRepository.ExistsByEmailAsync(request.Email))
                throw new InvalidOperationException("An account with this email already exists.");

            if (await _userRepository.ExistsByUsernameAsync(request.Username))
                throw new InvalidOperationException("This username is already taken.");

            var user = new User
            {
                Username = request.Username.Trim(),
                Email = request.Email.Trim().ToLowerInvariant(),
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password),
                Role = "user",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
            };

            await _userRepository.CreateAsync(user);
            return BuildAuthResponse(user);
        }

        public async Task<AuthResponseDto> LoginAsync(LoginRequestDto request)
        {
            var email = request.Email.Trim().ToLowerInvariant();
            var user = await _userRepository.GetByEmailAsync(email);

            if (user == null || !BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
                throw new UnauthorizedAccessException("Invalid email or password.");

            return BuildAuthResponse(user);
        }

        public async Task<UserProfileDto?> GetProfileAsync(int userId)
        {
            var user = await _userRepository.GetByIdAsync(userId);
            return user == null ? null : MapProfile(user);
        }

        private AuthResponseDto BuildAuthResponse(User user)
        {
            var (token, expiresAt) = _tokenService.CreateToken(user);
            return new AuthResponseDto
            {
                Token = token,
                ExpiresAt = expiresAt,
                User = MapProfile(user),
            };
        }

        private static UserProfileDto MapProfile(User user) => new()
        {
            Id = user.Id,
            Username = user.Username,
            Email = user.Email,
            Role = user.Role,
        };
    }
}

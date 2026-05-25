using CapstoneAPI.DTOs;
using CapstoneAPI.Models;

namespace CapstoneAPI.Services
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllUsersAsync();
        Task<UserDto?> GetUserByIdAsync(Guid id);
        Task<UserDto?> GetUserByEmailAsync(string email);
        Task<UserDto> CreateUserAsync(CreateUserDto userDto);
        Task<UserDto?> UpdateUserAsync(Guid id, UpdateUserDto userDto);
        Task<bool> DeleteUserAsync(Guid id);
        Task<bool> VerifyPasswordAsync(string email, string password);
    }
}

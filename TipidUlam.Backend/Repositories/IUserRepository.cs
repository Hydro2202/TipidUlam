using System.Threading.Tasks;
using TipidUlam.Backend.Models;

namespace TipidUlam.Backend.Repositories
{
    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdAsync(int id);
        Task<bool> ExistsByEmailAsync(string email);
        Task<bool> ExistsByUsernameAsync(string username);
        Task<User> CreateAsync(User user);
    }
}

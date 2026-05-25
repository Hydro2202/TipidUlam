using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TipidUlam.Backend.Data;
using TipidUlam.Backend.Models;

namespace TipidUlam.Backend.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly TipidUlamDbContext _context;

        public UserRepository(TipidUlamDbContext context)
        {
            _context = context;
        }

        public Task<User?> GetByEmailAsync(string email) =>
            _context.Users.FirstOrDefaultAsync(u => u.Email == email);

        public Task<User?> GetByIdAsync(int id) =>
            _context.Users.FirstOrDefaultAsync(u => u.Id == id);

        public Task<bool> ExistsByEmailAsync(string email) =>
            _context.Users.AnyAsync(u => u.Email == email);

        public Task<bool> ExistsByUsernameAsync(string username) =>
            _context.Users.AnyAsync(u => u.Username == username);

        public async Task<User> CreateAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }
    }
}

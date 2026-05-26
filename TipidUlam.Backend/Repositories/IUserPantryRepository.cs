using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TipidUlam.Backend.Data;
using TipidUlam.Backend.Models;

namespace TipidUlam.Backend.Repositories
{
    public interface IUserPantryRepository
    {
        Task<UserPantry> GetByIdAsync(int id);
        Task<IEnumerable<UserPantry>> GetByUserIdAsync(int userId);
        Task<UserPantry> GetUserIngredientAsync(int userId, int ingredientId);
        Task<UserPantry> AddAsync(UserPantry pantryItem);
        Task<UserPantry> UpdateAsync(UserPantry pantryItem);
        Task<bool> RemoveAsync(int id);
        Task<bool> RemoveByUserAndIngredientAsync(int userId, int ingredientId);
    }

    public class UserPantryRepository : IUserPantryRepository
    {
        private readonly TipidUlamDbContext _context;

        public UserPantryRepository(TipidUlamDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<UserPantry> GetByIdAsync(int id)
        {
            return await _context.UserPantry
                .Include(up => up.User)
                .Include(up => up.Ingredient)
                .FirstOrDefaultAsync(up => up.Id == id);
        }

        public async Task<IEnumerable<UserPantry>> GetByUserIdAsync(int userId)
        {
            return await _context.UserPantry
                .Include(up => up.Ingredient)
                .Where(up => up.UserId == userId)
                .OrderBy(up => up.Ingredient.Category)
                .ThenBy(up => up.Ingredient.Name)
                .ToListAsync();
        }

        public async Task<UserPantry> GetUserIngredientAsync(int userId, int ingredientId)
        {
            return await _context.UserPantry
                .Include(up => up.Ingredient)
                .FirstOrDefaultAsync(up => up.UserId == userId && up.IngredientId == ingredientId);
        }

        public async Task<UserPantry> AddAsync(UserPantry pantryItem)
        {
            _context.UserPantry.Add(pantryItem);
            await _context.SaveChangesAsync();
            return await GetByIdAsync(pantryItem.Id);
        }

        public async Task<UserPantry> UpdateAsync(UserPantry pantryItem)
        {
            pantryItem.UpdatedAt = DateTime.UtcNow;
            _context.UserPantry.Update(pantryItem);
            await _context.SaveChangesAsync();
            return await GetByIdAsync(pantryItem.Id);
        }

        public async Task<bool> RemoveAsync(int id)
        {
            var item = await _context.UserPantry.FindAsync(id);
            if (item == null)
                return false;

            _context.UserPantry.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveByUserAndIngredientAsync(int userId, int ingredientId)
        {
            var item = await _context.UserPantry
                .FirstOrDefaultAsync(up => up.UserId == userId && up.IngredientId == ingredientId);

            if (item == null)
                return false;

            _context.UserPantry.Remove(item);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

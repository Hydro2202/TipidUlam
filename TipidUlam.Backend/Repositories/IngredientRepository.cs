using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using TipidUlam.Backend.Data;
using TipidUlam.Backend.Models;

namespace TipidUlam.Backend.Repositories
{
    public class IngredientRepository : IIngredientRepository
    {
        private readonly TipidUlamDbContext _context;

        public IngredientRepository(TipidUlamDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Ingredient> GetByIdAsync(int id)
        {
            return await _context.Ingredients
                .Include(i => i.RecipeIngredients)
                .FirstOrDefaultAsync(i => i.Id == id);
        }

        public async Task<IEnumerable<Ingredient>> GetAllAsync()
        {
            return await _context.Ingredients.ToListAsync();
        }

        public async Task<IEnumerable<Ingredient>> GetByCategoryAsync(string category)
        {
            return await _context.Ingredients
                .Where(i => i.Category == category)
                .ToListAsync();
        }

        public async Task<Ingredient> CreateAsync(Ingredient ingredient)
        {
            _context.Ingredients.Add(ingredient);
            await _context.SaveChangesAsync();
            return ingredient;
        }

        public async Task<Ingredient> UpdateAsync(Ingredient ingredient)
        {
            ingredient.UpdatedAt = DateTime.UtcNow;
            _context.Ingredients.Update(ingredient);
            await _context.SaveChangesAsync();
            return ingredient;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var ingredient = await _context.Ingredients.FindAsync(id);
            if (ingredient == null)
                return false;

            _context.Ingredients.Remove(ingredient);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}

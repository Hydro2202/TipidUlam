using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TipidUlam.Backend.Models;

namespace TipidUlam.Backend.Repositories
{
    public interface IRecipeRepository
    {
        Task<Recipe> GetByIdAsync(int id);
        Task<IEnumerable<Recipe>> GetAllAsync();
        Task<IEnumerable<Recipe>> GetByCuisineTypeAsync(string cuisineType);
        Task<Recipe> CreateAsync(Recipe recipe);
        Task<Recipe> UpdateAsync(Recipe recipe);
        Task<bool> DeleteAsync(int id);
        Task SaveChangesAsync();
    }
}

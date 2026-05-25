using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TipidUlam.Backend.Models;

namespace TipidUlam.Backend.Repositories
{
    public interface IIngredientRepository
    {
        Task<Ingredient> GetByIdAsync(int id);
        Task<IEnumerable<Ingredient>> GetAllAsync();
        Task<IEnumerable<Ingredient>> GetByCategoryAsync(string category);
        Task<Ingredient> CreateAsync(Ingredient ingredient);
        Task<Ingredient> UpdateAsync(Ingredient ingredient);
        Task<bool> DeleteAsync(int id);
        Task SaveChangesAsync();
    }
}

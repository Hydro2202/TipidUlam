using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TipidUlam.Backend.DTOs;
using TipidUlam.Backend.Models;
using TipidUlam.Backend.Repositories;

namespace TipidUlam.Backend.Services
{
    public interface IIngredientService
    {
        Task<IngredientDto> GetByIdAsync(int id);
        Task<IEnumerable<IngredientDto>> GetAllAsync();
        Task<IEnumerable<IngredientDto>> GetByCategoryAsync(string category);
        Task<IngredientDto> CreateAsync(CreateUpdateIngredientDto dto);
        Task<IngredientDto> UpdateAsync(int id, CreateUpdateIngredientDto dto);
        Task<bool> DeleteAsync(int id);
    }

    public class IngredientService : IIngredientService
    {
        private readonly IIngredientRepository _ingredientRepository;

        public IngredientService(IIngredientRepository ingredientRepository)
        {
            _ingredientRepository = ingredientRepository ?? throw new ArgumentNullException(nameof(ingredientRepository));
        }

        public async Task<IngredientDto> GetByIdAsync(int id)
        {
            var ingredient = await _ingredientRepository.GetByIdAsync(id);
            if (ingredient == null)
                return null;

            return MapToDto(ingredient);
        }

        public async Task<IEnumerable<IngredientDto>> GetAllAsync()
        {
            var ingredients = await _ingredientRepository.GetAllAsync();
            return ingredients.Select(MapToDto);
        }

        public async Task<IEnumerable<IngredientDto>> GetByCategoryAsync(string category)
        {
            var ingredients = await _ingredientRepository.GetByCategoryAsync(category);
            return ingredients.Select(MapToDto);
        }

        public async Task<IngredientDto> CreateAsync(CreateUpdateIngredientDto dto)
        {
            var ingredient = new Ingredient
            {
                Name = dto.Name,
                UnitOfMeasure = dto.UnitOfMeasure,
                PricePerUnit = dto.PricePerUnit,
                Description = dto.Description,
                Category = dto.Category,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var created = await _ingredientRepository.CreateAsync(ingredient);
            return MapToDto(created);
        }

        public async Task<IngredientDto> UpdateAsync(int id, CreateUpdateIngredientDto dto)
        {
            var ingredient = await _ingredientRepository.GetByIdAsync(id);
            if (ingredient == null)
                return null;

            ingredient.Name = dto.Name;
            ingredient.UnitOfMeasure = dto.UnitOfMeasure;
            ingredient.PricePerUnit = dto.PricePerUnit;
            ingredient.Description = dto.Description;
            ingredient.Category = dto.Category;
            ingredient.UpdatedAt = DateTime.UtcNow;

            var updated = await _ingredientRepository.UpdateAsync(ingredient);
            return MapToDto(updated);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _ingredientRepository.DeleteAsync(id);
        }

        private IngredientDto MapToDto(Ingredient ingredient)
        {
            return new IngredientDto
            {
                Id = ingredient.Id,
                Name = ingredient.Name,
                UnitOfMeasure = ingredient.UnitOfMeasure,
                PricePerUnit = ingredient.PricePerUnit,
                Description = ingredient.Description,
                Category = ingredient.Category
            };
        }
    }
}

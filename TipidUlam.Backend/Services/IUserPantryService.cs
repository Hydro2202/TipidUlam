using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TipidUlam.Backend.DTOs;
using TipidUlam.Backend.Models;
using TipidUlam.Backend.Repositories;

namespace TipidUlam.Backend.Services
{
    public interface IUserPantryService
    {
        Task<IEnumerable<UserPantryItemDto>> GetUserPantryAsync(int userId);
        Task<UserPantryItemDto> AddToPantryAsync(int userId, AddToPantryDto dto);
        Task<UserPantryItemDto> UpdatePantryItemAsync(int userId, int pantryItemId, UpdatePantryItemDto dto);
        Task<bool> RemoveFromPantryAsync(int userId, int pantryItemId);
        Task<bool> RemoveByIngredientAsync(int userId, int ingredientId);
    }

    public class UserPantryService : IUserPantryService
    {
        private readonly IUserPantryRepository _pantryRepository;
        private readonly IIngredientRepository _ingredientRepository;

        public UserPantryService(IUserPantryRepository pantryRepository, IIngredientRepository ingredientRepository)
        {
            _pantryRepository = pantryRepository ?? throw new ArgumentNullException(nameof(pantryRepository));
            _ingredientRepository = ingredientRepository ?? throw new ArgumentNullException(nameof(ingredientRepository));
        }

        public async Task<IEnumerable<UserPantryItemDto>> GetUserPantryAsync(int userId)
        {
            var pantryItems = await _pantryRepository.GetByUserIdAsync(userId);
            return pantryItems.Select(MapToDto);
        }

        public async Task<UserPantryItemDto> AddToPantryAsync(int userId, AddToPantryDto dto)
        {
            // Check if ingredient exists
            var ingredient = await _ingredientRepository.GetByIdAsync(dto.IngredientId);
            if (ingredient == null)
                throw new InvalidOperationException($"Ingredient with ID {dto.IngredientId} not found.");

            // Check if already in pantry
            var existing = await _pantryRepository.GetUserIngredientAsync(userId, dto.IngredientId);
            if (existing != null)
                throw new InvalidOperationException("This ingredient is already in your pantry. Use update to modify the quantity.");

            var pantryItem = new UserPantry
            {
                UserId = userId,
                IngredientId = dto.IngredientId,
                Quantity = dto.Quantity,
                Notes = dto.Notes ?? string.Empty,
                AddedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            var added = await _pantryRepository.AddAsync(pantryItem);
            return MapToDto(added);
        }

        public async Task<UserPantryItemDto> UpdatePantryItemAsync(int userId, int pantryItemId, UpdatePantryItemDto dto)
        {
            var pantryItem = await _pantryRepository.GetByIdAsync(pantryItemId);
            if (pantryItem == null)
                throw new InvalidOperationException($"Pantry item with ID {pantryItemId} not found.");

            if (pantryItem.UserId != userId)
                throw new UnauthorizedAccessException("You do not have permission to update this pantry item.");

            pantryItem.Quantity = dto.Quantity;
            pantryItem.Notes = dto.Notes ?? string.Empty;

            var updated = await _pantryRepository.UpdateAsync(pantryItem);
            return MapToDto(updated);
        }

        public async Task<bool> RemoveFromPantryAsync(int userId, int pantryItemId)
        {
            var pantryItem = await _pantryRepository.GetByIdAsync(pantryItemId);
            if (pantryItem == null)
                return false;

            if (pantryItem.UserId != userId)
                throw new UnauthorizedAccessException("You do not have permission to remove this pantry item.");

            return await _pantryRepository.RemoveAsync(pantryItemId);
        }

        public async Task<bool> RemoveByIngredientAsync(int userId, int ingredientId)
        {
            return await _pantryRepository.RemoveByUserAndIngredientAsync(userId, ingredientId);
        }

        private UserPantryItemDto MapToDto(UserPantry pantryItem)
        {
            return new UserPantryItemDto
            {
                Id = pantryItem.Id,
                IngredientId = pantryItem.IngredientId,
                IngredientName = pantryItem.Ingredient?.Name,
                Category = pantryItem.Ingredient?.Category,
                UnitOfMeasure = pantryItem.Ingredient?.UnitOfMeasure,
                Quantity = pantryItem.Quantity,
                Notes = pantryItem.Notes
            };
        }
    }
}

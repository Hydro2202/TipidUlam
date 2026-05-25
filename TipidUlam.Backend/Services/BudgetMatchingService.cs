using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TipidUlam.Backend.DTOs;
using TipidUlam.Backend.Models;
using TipidUlam.Backend.Repositories;

namespace TipidUlam.Backend.Services
{
    public interface IBudgetMatchingService
    {
        /// <summary>
        /// Returns distinct recipes (one per dish) whose default household-scaled cost fits the budget.
        /// Costs use current ingredient prices from the database.
        /// </summary>
        Task<BudgetSearchResponseDto> SearchDistinctMealsAsync(BudgetSearchRequestDto request);

        /// <summary>
        /// Real-time cost for one recipe with optional proportional scaling from a user anchor weight.
        /// </summary>
        Task<RecipeCostCalculateResponseDto> CalculateRecipeCostAsync(
            int recipeId,
            RecipeCostCalculateRequestDto request);
    }

    public class BudgetMatchingService : IBudgetMatchingService
    {
        private readonly IRecipeRepository _recipeRepository;

        public BudgetMatchingService(IRecipeRepository recipeRepository)
        {
            _recipeRepository = recipeRepository ?? throw new ArgumentNullException(nameof(recipeRepository));
        }

        public async Task<BudgetSearchResponseDto> SearchDistinctMealsAsync(BudgetSearchRequestDto request)
        {
            ValidateBudget(request.MaxBudget);
            ValidateFamilySize(request.FamilySize);

            var pantryIds = request.PantryIngredientIds ?? new List<int>();
            var allRecipes = await _recipeRepository.GetAllAsync();

            // One suggestion per recipe ID — never multiple rows for the same base dish.
            var meals = allRecipes
                .GroupBy(r => r.Id)
                .Select(g => g.First())
                .Select(recipe => RecipeCostCalculator.BuildSuggestion(
                    recipe,
                    request.FamilySize,
                    request.MaxBudget,
                    pantryIds))
                .Where(s => s.FitsBudget)
                .OrderBy(s => s.TotalCost)
                .ThenBy(s => s.Name)
                .ToList();

            return new BudgetSearchResponseDto
            {
                MaxBudget = request.MaxBudget,
                FamilySize = request.FamilySize,
                PantryIngredientIds = pantryIds,
                Count = meals.Count,
                Meals = meals,
            };
        }

        public async Task<RecipeCostCalculateResponseDto> CalculateRecipeCostAsync(
            int recipeId,
            RecipeCostCalculateRequestDto request)
        {
            ValidateBudget(request.MaxBudget);
            ValidateFamilySize(request.FamilySize);

            var recipe = await _recipeRepository.GetByIdAsync(recipeId);
            if (recipe == null)
                throw new KeyNotFoundException($"Recipe with ID {recipeId} was not found.");

            var pantryIds = request.PantryIngredientIds ?? new List<int>();
            RecipeSuggestionDto meal;

            if (request.IngredientLines != null && request.IngredientLines.Count > 0)
            {
                meal = RecipeCostCalculator.BuildSuggestionWithLineOverrides(
                    recipe,
                    request.FamilySize,
                    request.MaxBudget,
                    pantryIds,
                    request.IngredientLines);
            }
            else if (request.AnchorIngredientId.HasValue && request.AnchorQuantity.HasValue)
            {
                if (!recipe.RecipeIngredients.Any(ri => ri.IngredientId == request.AnchorIngredientId.Value))
                    throw new ArgumentException("Anchor ingredient does not belong to this recipe.");

                meal = RecipeCostCalculator.BuildSuggestion(
                    recipe,
                    request.FamilySize,
                    request.MaxBudget,
                    pantryIds,
                    request.AnchorIngredientId,
                    request.AnchorQuantity);
            }
            else
            {
                throw new ArgumentException(
                    "Provide ingredientLines for manual edits, or anchorIngredientId and anchorQuantity for proportional scaling.");
            }

            return new RecipeCostCalculateResponseDto { Meal = meal };
        }

        private static void ValidateBudget(decimal maxBudget)
        {
            if (maxBudget <= 0)
                throw new ArgumentException("Budget must be greater than zero.", nameof(maxBudget));
        }

        private static void ValidateFamilySize(int familySize)
        {
            if (familySize <= 0)
                throw new ArgumentException("Family size must be greater than zero.", nameof(familySize));
        }
    }
}

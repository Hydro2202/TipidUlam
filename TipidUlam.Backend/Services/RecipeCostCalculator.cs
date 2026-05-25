using System;
using System.Collections.Generic;
using System.Linq;
using TipidUlam.Backend.DTOs;
using TipidUlam.Backend.Models;

namespace TipidUlam.Backend.Services
{
    /// <summary>
    /// Real-time recipe costing: baseline household quantities, proportional scaling from a user anchor weight,
    /// and line costs from current ingredient prices in the database.
    /// </summary>
    public static class RecipeCostCalculator
    {
        public static IReadOnlyDictionary<int, decimal> GetBaselineQuantities(Recipe recipe, int familySize)
        {
            if (recipe.BaseServings <= 0)
                throw new InvalidOperationException($"Recipe {recipe.Id} has invalid base servings.");

            var householdFactor = (decimal)familySize / recipe.BaseServings;

            return recipe.RecipeIngredients.ToDictionary(
                ri => ri.IngredientId,
                ri => Math.Round(ri.QuantityPerServing * householdFactor, 4));
        }

        /// <summary>
        /// Scales every ingredient proportionally when the user sets a custom amount on one anchor ingredient.
        /// </summary>
        public static IReadOnlyDictionary<int, decimal> ApplyAnchorScaling(
            IReadOnlyDictionary<int, decimal> baselineQuantities,
            int anchorIngredientId,
            decimal anchorQuantity)
        {
            if (!baselineQuantities.TryGetValue(anchorIngredientId, out var anchorBaseline) || anchorBaseline <= 0)
                throw new ArgumentException(
                    "Anchor ingredient is not part of this recipe or has no baseline quantity.",
                    nameof(anchorIngredientId));

            if (anchorQuantity <= 0)
                throw new ArgumentException("Anchor quantity must be greater than zero.", nameof(anchorQuantity));

            var scaleFactor = anchorQuantity / anchorBaseline;

            return baselineQuantities.ToDictionary(
                kvp => kvp.Key,
                kvp => Math.Round(kvp.Value * scaleFactor, 4));
        }

        public static RecipeSuggestionDto BuildSuggestion(
            Recipe recipe,
            int familySize,
            decimal maxBudget,
            IReadOnlyCollection<int> pantryIngredientIds,
            int? anchorIngredientId = null,
            decimal? anchorQuantity = null)
        {
            var pantry = pantryIngredientIds as HashSet<int> ?? pantryIngredientIds.ToHashSet();
            var baselines = GetBaselineQuantities(recipe, familySize);

            IReadOnlyDictionary<int, decimal> requiredQuantities;
            decimal scaleFactor = 1m;

            if (anchorIngredientId.HasValue && anchorQuantity.HasValue)
            {
                requiredQuantities = ApplyAnchorScaling(baselines, anchorIngredientId.Value, anchorQuantity.Value);
                scaleFactor = anchorQuantity.Value / baselines[anchorIngredientId.Value];
                scaleFactor = Math.Round(scaleFactor, 4);
            }
            else
            {
                requiredQuantities = baselines;
            }

            var lines = new List<RecipeIngredientLineDto>();
            decimal totalCost = 0m;

            foreach (var recipeIngredient in recipe.RecipeIngredients.OrderBy(ri => ri.Ingredient.Name))
            {
                var ingredient = recipeIngredient.Ingredient;
                var requiredQty = requiredQuantities[ingredient.Id];
                var baselineQty = baselines[ingredient.Id];
                var isPantry = pantry.Contains(ingredient.Id);
                var pricePerBase = ingredient.PricePerUnit;
                var lineCost = isPantry ? 0m : Math.Round(requiredQty * pricePerBase, 2);

                totalCost += lineCost;

                lines.Add(new RecipeIngredientLineDto
                {
                    RecipeIngredientId = recipeIngredient.Id,
                    IngredientId = ingredient.Id,
                    IngredientName = ingredient.Name,
                    UnitOfMeasure = ingredient.UnitOfMeasure,
                    PricePerBaseUnit = pricePerBase,
                    BaselineQuantity = baselineQty,
                    RequiredQuantity = requiredQty,
                    LineCost = lineCost,
                    IsPantryItem = isPantry,
                    IsAnchorIngredient = anchorIngredientId == ingredient.Id,
                });
            }

            totalCost = Math.Round(totalCost, 2);
            var fits = totalCost <= maxBudget;

            return new RecipeSuggestionDto
            {
                Id = recipe.Id,
                Name = recipe.Name,
                Description = recipe.Description,
                Instructions = recipe.Instructions,
                BaseServings = recipe.BaseServings,
                CuisineType = recipe.CuisineType,
                DifficultyLevel = recipe.DifficultyLevel,
                CookingTimeMinutes = recipe.CookingTimeMinutes,
                ScaleFactor = scaleFactor,
                TotalCost = totalCost,
                FitsBudget = fits,
                BudgetRemaining = Math.Round(maxBudget - totalCost, 2),
                Ingredients = lines,
            };
        }

        /// <summary>
        /// Uses user-supplied quantities and optional per-ingredient prices (e.g. palengke prices).
        /// </summary>
        public static RecipeSuggestionDto BuildSuggestionWithLineOverrides(
            Recipe recipe,
            int familySize,
            decimal maxBudget,
            IReadOnlyCollection<int> pantryIngredientIds,
            IReadOnlyList<IngredientLineOverrideDto> lineOverrides)
        {
            if (lineOverrides == null || lineOverrides.Count == 0)
                throw new ArgumentException("At least one ingredient line override is required.", nameof(lineOverrides));

            var pantry = pantryIngredientIds as HashSet<int> ?? pantryIngredientIds.ToHashSet();
            var baselines = GetBaselineQuantities(recipe, familySize);
            var overrideByIngredient = lineOverrides
                .GroupBy(o => o.IngredientId)
                .ToDictionary(g => g.Key, g => g.Last());

            var recipeIngredientIds = recipe.RecipeIngredients.Select(ri => ri.IngredientId).ToHashSet();
            foreach (var ingredientId in overrideByIngredient.Keys)
            {
                if (!recipeIngredientIds.Contains(ingredientId))
                    throw new ArgumentException($"Ingredient {ingredientId} is not part of this recipe.");
            }

            var lines = new List<RecipeIngredientLineDto>();
            decimal totalCost = 0m;
            decimal scaleFactor = 1m;

            foreach (var recipeIngredient in recipe.RecipeIngredients.OrderBy(ri => ri.Ingredient.Name))
            {
                var ingredient = recipeIngredient.Ingredient;
                var baselineQty = baselines[ingredient.Id];

                if (!overrideByIngredient.TryGetValue(ingredient.Id, out var lineOverride))
                    throw new ArgumentException(
                        $"Missing quantity for ingredient '{ingredient.Name}'. Include all recipe ingredients.");

                var requiredQty = Math.Round(lineOverride.RequiredQuantity, 4);
                var pricePerBase = lineOverride.PricePerBaseUnit ?? ingredient.PricePerUnit;
                var isPantry = pantry.Contains(ingredient.Id);
                var lineCost = isPantry ? 0m : Math.Round(requiredQty * pricePerBase, 2);

                totalCost += lineCost;

                if (baselineQty > 0)
                {
                    var lineScale = requiredQty / baselineQty;
                    if (lineScale > scaleFactor)
                        scaleFactor = Math.Round(lineScale, 4);
                }

                lines.Add(new RecipeIngredientLineDto
                {
                    RecipeIngredientId = recipeIngredient.Id,
                    IngredientId = ingredient.Id,
                    IngredientName = ingredient.Name,
                    UnitOfMeasure = ingredient.UnitOfMeasure,
                    PricePerBaseUnit = pricePerBase,
                    BaselineQuantity = baselineQty,
                    RequiredQuantity = requiredQty,
                    LineCost = lineCost,
                    IsPantryItem = isPantry,
                    IsAnchorIngredient = false,
                });
            }

            totalCost = Math.Round(totalCost, 2);

            return new RecipeSuggestionDto
            {
                Id = recipe.Id,
                Name = recipe.Name,
                Description = recipe.Description,
                Instructions = recipe.Instructions,
                BaseServings = recipe.BaseServings,
                CuisineType = recipe.CuisineType,
                DifficultyLevel = recipe.DifficultyLevel,
                CookingTimeMinutes = recipe.CookingTimeMinutes,
                ScaleFactor = scaleFactor,
                TotalCost = totalCost,
                FitsBudget = totalCost <= maxBudget,
                BudgetRemaining = Math.Round(maxBudget - totalCost, 2),
                Ingredients = lines,
            };
        }
    }
}

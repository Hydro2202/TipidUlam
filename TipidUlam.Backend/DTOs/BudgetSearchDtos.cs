using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TipidUlam.Backend.DTOs
{
    /// <summary>
    /// Budget search accepts any positive decimal amount (e.g. 75, 150, 1250).
    /// </summary>
    public class BudgetSearchRequestDto
    {
        [Range(typeof(decimal), "0.01", "999999999")]
        public decimal MaxBudget { get; set; }

        [Range(1, 50)]
        public int FamilySize { get; set; } = 4;

        public List<int> PantryIngredientIds { get; set; } = new();
    }

    public class BudgetSearchResponseDto
    {
        public decimal MaxBudget { get; set; }
        public int FamilySize { get; set; }
        public IReadOnlyList<int> PantryIngredientIds { get; set; } = new List<int>();
        public int Count { get; set; }
        /// <summary>
        /// Distinct recipes only — one entry per dish, no duplicate base-meal variations.
        /// </summary>
        public IReadOnlyList<RecipeSuggestionDto> Meals { get; set; } = new List<RecipeSuggestionDto>();
    }

    /// <summary>
    /// A single meal suggestion with costs computed from current DB prices at request time.
    /// </summary>
    public class RecipeSuggestionDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string Instructions { get; set; } = string.Empty;
        public int BaseServings { get; set; }
        public string? CuisineType { get; set; }
        public string? DifficultyLevel { get; set; }
        public int? CookingTimeMinutes { get; set; }

        /// <summary>1 = default household scaling; changes when user anchors an ingredient weight.</summary>
        public decimal ScaleFactor { get; set; } = 1m;

        public decimal TotalCost { get; set; }
        public bool FitsBudget { get; set; }
        public decimal BudgetRemaining { get; set; }

        public IReadOnlyList<RecipeIngredientLineDto> Ingredients { get; set; } = new List<RecipeIngredientLineDto>();
    }

    public class RecipeIngredientLineDto
    {
        public int RecipeIngredientId { get; set; }
        public int IngredientId { get; set; }
        public string IngredientName { get; set; } = string.Empty;
        public string UnitOfMeasure { get; set; } = string.Empty;

        /// <summary>Market price for one base unit (e.g. per 1 kg) from the database at calculation time.</summary>
        public decimal PricePerBaseUnit { get; set; }

        /// <summary>Quantity for this household before any user anchor adjustment.</summary>
        public decimal BaselineQuantity { get; set; }

        /// <summary>Quantity after household + proportional scaling.</summary>
        public decimal RequiredQuantity { get; set; }

        /// <summary>RequiredQuantity × PricePerBaseUnit; zero when ingredient is in the pantry.</summary>
        public decimal LineCost { get; set; }

        public bool IsPantryItem { get; set; }
        public bool IsAnchorIngredient { get; set; }
    }
}

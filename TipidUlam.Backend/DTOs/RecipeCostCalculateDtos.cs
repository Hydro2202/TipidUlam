using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TipidUlam.Backend.DTOs
{
    public class IngredientLineOverrideDto
    {
        [Required]
        public int IngredientId { get; set; }

        [Range(typeof(decimal), "0.0001", "999999")]
        public decimal RequiredQuantity { get; set; }

        /// <summary>Override market price per base unit (e.g. per kg). Omit to use DB price.</summary>
        [Range(typeof(decimal), "0.01", "999999")]
        public decimal? PricePerBaseUnit { get; set; }
    }

    public class RecipeCostCalculateRequestDto
    {
        [Range(typeof(decimal), "0.01", "999999999")]
        public decimal MaxBudget { get; set; }

        [Range(1, 50)]
        public int FamilySize { get; set; } = 4;

        public List<int> PantryIngredientIds { get; set; } = new();

        /// <summary>
        /// Per-line qty and price edits from the user. When set, each line uses these values directly.
        /// </summary>
        public List<IngredientLineOverrideDto>? IngredientLines { get; set; }

        /// <summary>Optional: scale all lines proportionally from one anchor (used when IngredientLines is empty).</summary>
        public int? AnchorIngredientId { get; set; }

        [Range(typeof(decimal), "0.0001", "999999")]
        public decimal? AnchorQuantity { get; set; }
    }

    public class RecipeCostCalculateResponseDto
    {
        public RecipeSuggestionDto Meal { get; set; } = new();
    }
}

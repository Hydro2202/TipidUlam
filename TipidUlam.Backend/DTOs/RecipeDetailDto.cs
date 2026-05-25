using System;
using System.Collections.Generic;

namespace TipidUlam.Backend.DTOs
{
    public class RecipeDetailDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Instructions { get; set; }
        public int BaseServings { get; set; }
        public string CuisineType { get; set; }
        public string DifficultyLevel { get; set; }
        public int? CookingTimeMinutes { get; set; }
        public decimal TotalCostForFamily { get; set; } // Total cost after scaling and pantry adjustments
        public List<RecipeIngredientDetailDto> Ingredients { get; set; } = new List<RecipeIngredientDetailDto>();
    }
}

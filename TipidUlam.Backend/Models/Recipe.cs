using System;
using System.Collections.Generic;

namespace TipidUlam.Backend.Models
{
    public class Recipe
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Instructions { get; set; }
        public int BaseServings { get; set; } = 2; // Base serving size for the recipe
        public string CuisineType { get; set; } // 'Filipino', 'Asian', 'Western', etc.
        public string DifficultyLevel { get; set; } // 'Easy', 'Medium', 'Hard'
        public int? CookingTimeMinutes { get; set; }
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
    }
}

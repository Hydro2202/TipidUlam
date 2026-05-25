using System;

namespace TipidUlam.Backend.Models
{
    public class RecipeIngredient
    {
        public int Id { get; set; }
        public int RecipeId { get; set; }
        public int IngredientId { get; set; }
        public decimal QuantityPerServing { get; set; } // Quantity per serving unit
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public Recipe Recipe { get; set; }
        public Ingredient Ingredient { get; set; }
    }
}

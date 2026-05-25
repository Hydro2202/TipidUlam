using System;
using System.Collections.Generic;

namespace TipidUlam.Backend.Models
{
    public class Ingredient
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UnitOfMeasure { get; set; } // 'piece', 'kg', 'liter', 'cup', 'tbsp', etc.
        /// <summary>Price in PHP for one base unit of measure (e.g. per 1 kg, per 1 liter).</summary>
        public decimal PricePerUnit { get; set; }
        public string Description { get; set; }
        public string Category { get; set; } // 'vegetable', 'protein', 'grain', 'spice', etc.
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
        public ICollection<IngredientPriceHistory> PriceHistory { get; set; } = new List<IngredientPriceHistory>();
    }
}

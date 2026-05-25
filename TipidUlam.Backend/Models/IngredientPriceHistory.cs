using System;

namespace TipidUlam.Backend.Models
{
    public class IngredientPriceHistory
    {
        public int Id { get; set; }
        public int IngredientId { get; set; }
        public decimal OldPrice { get; set; }
        public decimal NewPrice { get; set; }
        public int? ChangedBy { get; set; } // User ID who changed the price
        
        public DateTime ChangedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public Ingredient Ingredient { get; set; }
        public User ChangedByUser { get; set; }
    }
}

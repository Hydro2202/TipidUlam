using System;

namespace TipidUlam.Backend.Models
{
    public class UserPantry
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int IngredientId { get; set; }
        
        /// <summary>Quantity of ingredient the user has (in the unit of measure).</summary>
        public decimal Quantity { get; set; }
        
        /// <summary>Notes about the ingredient (e.g., "expiring soon", "from market").</summary>
        public string Notes { get; set; }
        
        public DateTime AddedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public User User { get; set; }
        public Ingredient Ingredient { get; set; }
    }
}

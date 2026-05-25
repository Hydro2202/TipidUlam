using System;
using System.Collections.Generic;

namespace TipidUlam.Backend.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string Role { get; set; } = "user"; // 'user' or 'admin'
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation properties
        public ICollection<IngredientPriceHistory> PriceHistoryChanges { get; set; } = new List<IngredientPriceHistory>();
    }
}

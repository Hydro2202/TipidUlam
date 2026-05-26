using System.ComponentModel.DataAnnotations;

namespace TipidUlam.Backend.DTOs
{
    public class UserPantryItemDto
    {
        public int Id { get; set; }
        public int IngredientId { get; set; }
        public string IngredientName { get; set; }
        public string Category { get; set; }
        public string UnitOfMeasure { get; set; }
        public decimal Quantity { get; set; }
        public string Notes { get; set; }
    }

    public class AddToPantryDto
    {
        [Required]
        public int IngredientId { get; set; }

        [Required, Range(0.001, double.MaxValue)]
        public decimal Quantity { get; set; }

        public string Notes { get; set; } = string.Empty;
    }

    public class UpdatePantryItemDto
    {
        [Required, Range(0.001, double.MaxValue)]
        public decimal Quantity { get; set; }

        public string Notes { get; set; } = string.Empty;
    }
}

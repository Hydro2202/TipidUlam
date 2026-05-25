namespace TipidUlam.Backend.DTOs
{
    public class RecipeIngredientDetailDto
    {
        public int Id { get; set; }
        public int IngredientId { get; set; }
        public string IngredientName { get; set; }
        public string UnitOfMeasure { get; set; }
        public decimal PricePerUnit { get; set; }
        public decimal QuantityPerServing { get; set; }
    }
}

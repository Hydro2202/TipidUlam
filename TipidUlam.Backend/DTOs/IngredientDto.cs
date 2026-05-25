namespace TipidUlam.Backend.DTOs
{
    public class IngredientDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string UnitOfMeasure { get; set; }
        public decimal PricePerUnit { get; set; }
        public decimal PricePerBaseUnit => PricePerUnit;
        public string Description { get; set; }
        public string Category { get; set; }
    }
}

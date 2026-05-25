namespace TipidUlam.Backend.DTOs
{
    public class CreateUpdateIngredientDto
    {
        public string Name { get; set; }
        public string UnitOfMeasure { get; set; }
        public decimal PricePerUnit { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
    }
}

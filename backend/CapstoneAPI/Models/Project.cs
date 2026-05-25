namespace CapstoneAPI.Models
{
    public class Project
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Guid OwnerId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation properties
        public User? Owner { get; set; }
        public ICollection<Task> Tasks { get; set; } = new List<Task>();
    }
}

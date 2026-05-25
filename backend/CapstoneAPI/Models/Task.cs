namespace CapstoneAPI.Models
{
    public class Task
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public Guid ProjectId { get; set; }
        public Guid? AssignedToId { get; set; }
        public string Status { get; set; } = "Not Started";
        public string Priority { get; set; } = "Medium";
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        // Navigation properties
        public Project? Project { get; set; }
        public User? AssignedTo { get; set; }
    }
}

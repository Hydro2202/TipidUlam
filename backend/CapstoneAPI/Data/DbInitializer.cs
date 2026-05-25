using CapstoneAPI.Models;

namespace CapstoneAPI.Data
{
    public static class DbInitializer
    {
        public static void Initialize(CapstoneDbContext context)
        {
            if (context.Users.Any())
            {
                return; // Database already has data
            }

            // Seed users
            var users = new[]
            {
                new User
                {
                    Id = Guid.NewGuid(),
                    Name = "John Developer",
                    Email = "john@example.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                    CreatedAt = DateTime.UtcNow
                },
                new User
                {
                    Id = Guid.NewGuid(),
                    Name = "Jane Manager",
                    Email = "jane@example.com",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("password123"),
                    CreatedAt = DateTime.UtcNow
                }
            };

            context.Users.AddRange(users);
            context.SaveChanges();

            // Seed projects
            var projects = new[]
            {
                new Project
                {
                    Id = Guid.NewGuid(),
                    Name = "Mobile App Development",
                    Description = "Building a cross-platform mobile application",
                    OwnerId = users[0].Id,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Project
                {
                    Id = Guid.NewGuid(),
                    Name = "Web Portal Redesign",
                    Description = "Redesigning the customer web portal",
                    OwnerId = users[1].Id,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };

            context.Projects.AddRange(projects);
            context.SaveChanges();

            // Seed tasks
            var tasks = new[]
            {
                new Task
                {
                    Id = Guid.NewGuid(),
                    Title = "Setup project infrastructure",
                    Description = "Configure CI/CD and development environment",
                    ProjectId = projects[0].Id,
                    AssignedToId = users[0].Id,
                    Status = "In Progress",
                    Priority = "High",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                },
                new Task
                {
                    Id = Guid.NewGuid(),
                    Title = "Design database schema",
                    Description = "Create database models and relationships",
                    ProjectId = projects[1].Id,
                    AssignedToId = users[1].Id,
                    Status = "Completed",
                    Priority = "High",
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                }
            };

            context.Tasks.AddRange(tasks);
            context.SaveChanges();
        }
    }
}

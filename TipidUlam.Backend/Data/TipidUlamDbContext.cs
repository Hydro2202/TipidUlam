using Microsoft.EntityFrameworkCore;
using TipidUlam.Backend.Models;

namespace TipidUlam.Backend.Data
{
    public class TipidUlamDbContext : DbContext
    {
        public TipidUlamDbContext(DbContextOptions<TipidUlamDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Ingredient> Ingredients { get; set; }
        public DbSet<Recipe> Recipes { get; set; }
        public DbSet<RecipeIngredient> RecipeIngredients { get; set; }
        public DbSet<IngredientPriceHistory> IngredientPriceHistory { get; set; }
        public DbSet<UserPantry> UserPantry { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Ingredient>(entity =>
            {
                entity.ToTable("ingredients");
                entity.HasKey(i => i.Id);
                entity.Property(i => i.Id).HasColumnName("id");
                entity.Property(i => i.Name).HasColumnName("name").IsRequired().HasMaxLength(255);
                entity.Property(i => i.UnitOfMeasure).HasColumnName("unit_of_measure").IsRequired().HasMaxLength(50);
                entity.Property(i => i.PricePerUnit).HasColumnName("price_per_unit").HasPrecision(10, 2);
                entity.Property(i => i.Description).HasColumnName("description");
                entity.Property(i => i.Category).HasColumnName("category").HasMaxLength(100);
                entity.Property(i => i.CreatedAt).HasColumnName("created_at");
                entity.Property(i => i.UpdatedAt).HasColumnName("updated_at");
                entity.HasIndex(i => i.Category);
            });

            modelBuilder.Entity<Recipe>(entity =>
            {
                entity.ToTable("recipes");
                entity.HasKey(r => r.Id);
                entity.Property(r => r.Id).HasColumnName("id");
                entity.Property(r => r.Name).HasColumnName("name").IsRequired().HasMaxLength(255);
                entity.Property(r => r.Description).HasColumnName("description");
                entity.Property(r => r.Instructions).HasColumnName("instructions").IsRequired();
                entity.Property(r => r.BaseServings).HasColumnName("base_servings");
                entity.Property(r => r.CuisineType).HasColumnName("cuisine_type").HasMaxLength(100);
                entity.Property(r => r.DifficultyLevel).HasColumnName("difficulty_level").HasMaxLength(50);
                entity.Property(r => r.CookingTimeMinutes).HasColumnName("cooking_time_minutes");
                entity.Property(r => r.CreatedAt).HasColumnName("created_at");
                entity.Property(r => r.UpdatedAt).HasColumnName("updated_at");
                entity.HasIndex(r => r.CuisineType);
            });

            modelBuilder.Entity<RecipeIngredient>(entity =>
            {
                entity.ToTable("recipe_ingredients");
                entity.HasKey(ri => ri.Id);
                entity.Property(ri => ri.Id).HasColumnName("id");
                entity.Property(ri => ri.RecipeId).HasColumnName("recipe_id");
                entity.Property(ri => ri.IngredientId).HasColumnName("ingredient_id");
                entity.Property(ri => ri.QuantityPerServing).HasColumnName("quantity_per_serving").HasPrecision(10, 3);
                entity.Property(ri => ri.CreatedAt).HasColumnName("created_at");
                entity.Property(ri => ri.UpdatedAt).HasColumnName("updated_at");
                entity.HasOne(ri => ri.Recipe)
                    .WithMany(r => r.RecipeIngredients)
                    .HasForeignKey(ri => ri.RecipeId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(ri => ri.Ingredient)
                    .WithMany(i => i.RecipeIngredients)
                    .HasForeignKey(ri => ri.IngredientId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasIndex(ri => ri.RecipeId);
                entity.HasIndex(ri => ri.IngredientId);
                entity.HasAlternateKey(ri => new { ri.RecipeId, ri.IngredientId });
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Id).HasColumnName("id");
                entity.Property(u => u.Username).HasColumnName("username").IsRequired().HasMaxLength(255);
                entity.Property(u => u.Email).HasColumnName("email").IsRequired().HasMaxLength(255);
                entity.Property(u => u.PasswordHash).HasColumnName("password_hash").IsRequired().HasMaxLength(255);
                entity.Property(u => u.Role).HasColumnName("role").IsRequired().HasMaxLength(50);
                entity.Property(u => u.CreatedAt).HasColumnName("created_at");
                entity.Property(u => u.UpdatedAt).HasColumnName("updated_at");
                entity.HasIndex(u => u.Username).IsUnique();
                entity.HasIndex(u => u.Email).IsUnique();
                entity.HasIndex(u => u.Role);
            });

            modelBuilder.Entity<IngredientPriceHistory>(entity =>
            {
                entity.ToTable("ingredient_price_history");
                entity.HasKey(ph => ph.Id);
                entity.Property(ph => ph.Id).HasColumnName("id");
                entity.Property(ph => ph.IngredientId).HasColumnName("ingredient_id");
                entity.Property(ph => ph.OldPrice).HasColumnName("old_price").HasPrecision(10, 2);
                entity.Property(ph => ph.NewPrice).HasColumnName("new_price").HasPrecision(10, 2);
                entity.Property(ph => ph.ChangedBy).HasColumnName("changed_by");
                entity.Property(ph => ph.ChangedAt).HasColumnName("changed_at");
                entity.HasOne(ph => ph.Ingredient)
                    .WithMany(i => i.PriceHistory)
                    .HasForeignKey(ph => ph.IngredientId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(ph => ph.ChangedByUser)
                    .WithMany(u => u.PriceHistoryChanges)
                    .HasForeignKey(ph => ph.ChangedBy)
                    .OnDelete(DeleteBehavior.SetNull);
                entity.HasIndex(ph => ph.IngredientId);
            });

            modelBuilder.Entity<UserPantry>(entity =>
            {
                entity.ToTable("user_pantry");
                entity.HasKey(up => up.Id);
                entity.Property(up => up.Id).HasColumnName("id");
                entity.Property(up => up.UserId).HasColumnName("user_id");
                entity.Property(up => up.IngredientId).HasColumnName("ingredient_id");
                entity.Property(up => up.Quantity).HasColumnName("quantity").HasPrecision(10, 3);
                entity.Property(up => up.Notes).HasColumnName("notes").HasMaxLength(500);
                entity.Property(up => up.AddedAt).HasColumnName("added_at");
                entity.Property(up => up.UpdatedAt).HasColumnName("updated_at");
                entity.HasOne(up => up.User)
                    .WithMany(u => u.PantryItems)
                    .HasForeignKey(up => up.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasOne(up => up.Ingredient)
                    .WithMany()
                    .HasForeignKey(up => up.IngredientId)
                    .OnDelete(DeleteBehavior.Cascade);
                entity.HasIndex(up => up.UserId);
                entity.HasIndex(up => up.IngredientId);
                entity.HasAlternateKey(up => new { up.UserId, up.IngredientId });
            });
        }
    }
}

using System;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace TipidUlam.Backend.Data
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(IServiceProvider services)
        {
            using var scope = services.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<TipidUlamDbContext>();
            var env = scope.ServiceProvider.GetRequiredService<IHostEnvironment>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<TipidUlamDbContext>>();

            await context.Database.EnsureCreatedAsync();

            var recipeCount = await context.Recipes.CountAsync();
            const int expectedSampleRecipeCount = 14;

            if (recipeCount >= expectedSampleRecipeCount && await context.Ingredients.AnyAsync())
            {
                logger.LogInformation("Database already contains sample data ({RecipeCount} recipes).", recipeCount);
                return;
            }

            if (recipeCount > 0 && recipeCount < expectedSampleRecipeCount)
            {
                logger.LogInformation(
                    "Database contains {RecipeCount}/{ExpectedRecipeCount} sample recipes. Recreating and reloading sample data.",
                    recipeCount,
                    expectedSampleRecipeCount);
                await context.Database.EnsureDeletedAsync();
                await context.Database.EnsureCreatedAsync();
            }

            var sampleDataPath = Path.Combine(env.ContentRootPath, "sample-data.sql");
            if (!File.Exists(sampleDataPath))
            {
                logger.LogWarning("sample-data.sql not found at {Path}", sampleDataPath);
                return;
            }

            var sql = await File.ReadAllTextAsync(sampleDataPath);
            var executableSql = string.Join(
                Environment.NewLine,
                sql.Split('\n')
                    .Where(line =>
                    {
                        var trimmedLine = line.Trim();
                        return trimmedLine.Length > 0 &&
                               !trimmedLine.StartsWith("--", StringComparison.Ordinal) &&
                               !trimmedLine.StartsWith("SELECT", StringComparison.OrdinalIgnoreCase);
                    }));

            var connection = context.Database.GetDbConnection();
            if (connection.State != ConnectionState.Open)
                await connection.OpenAsync();

            var statements = executableSql.Split(';', StringSplitOptions.RemoveEmptyEntries);
            foreach (var statement in statements)
            {
                var trimmed = statement.Trim();
                if (string.IsNullOrWhiteSpace(trimmed)) continue;

                await using var command = connection.CreateCommand();
                command.CommandText = trimmed;
                await command.ExecuteNonQueryAsync();
            }

            logger.LogInformation("Sample data loaded successfully ({Count} statements).", statements.Length);
        }
    }
}

namespace TipidUlam.Backend.Configuration
{
    public static class DatabaseSettings
    {
        public static string BuildConnectionString(IConfiguration configuration)
        {
            var host = configuration["DB_HOST"] ?? "localhost";
            var port = configuration["DB_PORT"] ?? "5432";
            var database = configuration["DB_NAME"] ?? "TipidUlam";
            var username = configuration["DB_USER"] ?? "Raiden";
            var password = configuration["DB_PASSWORD"] ?? "Raiden123456789";

            return $"Host={host};Port={port};Database={database};Username={username};Password={password}";
        }
    }
}

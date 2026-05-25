using TipidUlam.Backend;
using TipidUlam.Backend.Data;

LoadEnvFile(Path.Combine(AppContext.BaseDirectory, ".env"));
LoadEnvFile(Path.Combine(Directory.GetCurrentDirectory(), ".env"));

var builder = WebApplication.CreateBuilder(args);

var dbConnection = TipidUlam.Backend.Configuration.DatabaseSettings.BuildConnectionString(builder.Configuration);
builder.Configuration["ConnectionStrings:DefaultConnection"] = dbConnection;

static void LoadEnvFile(string path)
{
    if (!File.Exists(path)) return;

    foreach (var line in File.ReadAllLines(path))
    {
        var trimmed = line.Trim();
        if (trimmed.Length == 0 || trimmed.StartsWith('#')) continue;

        var separatorIndex = trimmed.IndexOf('=');
        if (separatorIndex <= 0) continue;

        var key = trimmed[..separatorIndex].Trim();
        var value = trimmed[(separatorIndex + 1)..].Trim();
        Environment.SetEnvironmentVariable(key, value);
    }
}

// Load startup configuration
var startup = new Startup(builder.Configuration);
startup.ConfigureServices(builder.Services);

var app = builder.Build();

await DbInitializer.InitializeAsync(app.Services);

// Configure the HTTP request pipeline
startup.Configure(app, app.Environment);

app.Run();

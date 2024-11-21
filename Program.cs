using Microsoft.Azure.Cosmos;
using TaskApplicationApi;
using TaskApplicationApi.Models;
using TaskApplicationApi.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: true)
    .AddUserSecrets<Program>();

var startup = new Startup(builder.Configuration);

// Add services to the container.
startup.ConfigureServices(builder.Services);

var app = builder.Build();

app.MapControllers();

app.Run();
using ClubSyncAPI.Database;
using ClubSyncAPI.Database.Seeders;
using ClubSyncAPI.Endpoints;
using Microsoft.EntityFrameworkCore;
var allowedRequestOrigins = "ClubSyncApp";
var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();
var connectionString = builder.Configuration.GetConnectionString("ClubSyncDb") ?? throw new InvalidOperationException("Connection string 'ClubSyncDb' not found.");
builder.Services.AddDbContext<ClubSyncDbContext>(options => options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention());
builder.Services.AddDbContext<ClubSyncIdentityDbContext>(options => options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention());
builder.Services.AddIdentity<User, Role>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequiredLength = 10;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequireUppercase = true;
    options.Password.RequiredUniqueChars = 1;
}).AddEntityFrameworkStores<ClubSyncIdentityDbContext>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: allowedRequestOrigins,
    policy =>
    {
        policy.WithOrigins("http://localhost:4200", "http://127.0.0.1:4200");
    });
});

var app = builder.Build();
app.MapEndpoints();
app.MapGet("/", () => new { Title = "ClubSync Admin App" });
app.UseCors(allowedRequestOrigins);
using (var scope = app.Services.CreateScope())
{
    await SeedRootUser.RunAsync(scope.ServiceProvider);
}
app.Run();

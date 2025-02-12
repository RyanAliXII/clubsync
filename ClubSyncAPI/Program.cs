using ClubSyncAPI.Database;
using ClubSyncAPI.Database.Seeders;
using ClubSyncAPI.Endpoints;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

using System.Text;
using ClubSyncAPI.Features.Users.Infrastructure;
using Microsoft.AspNetCore.Identity;

var allowedRequestOrigins = "ClubSyncApp";
var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddEnvironmentVariables();

var config = builder.Configuration;

builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidIssuer = config["JwtSettings:Issuer"],
        ValidAudience = config["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["JwtSettings:SecretKey"]!)),
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true

    };
});
var connectionString = config.GetConnectionString("ClubSyncDb") ?? throw new InvalidOperationException("Connection string 'ClubSyncDb' not found.");

builder.Services.AddDbContext<ClubSyncDbContext>(options => options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention());
builder.Services.AddDbContext<ClubSyncIdentityDbContext>(options => options.UseNpgsql(connectionString).UseSnakeCaseNamingConvention());

builder.Services.AddSingleton<TokenProvider>();
builder.Services
.AddIdentityCore<User>()
.AddRoles<Role>()
.AddSignInManager()
.AddEntityFrameworkStores<ClubSyncIdentityDbContext>()
.AddTokenProvider<DataProtectorTokenProvider<User>>("REFRESHTOKENPROVIDER");
builder.Services.Configure<DataProtectionTokenProviderOptions>(options =>
{
    options.TokenLifespan = TimeSpan.FromDays(1);
});
builder.Services.AddCors(options =>
{
    options.AddPolicy(name: allowedRequestOrigins,
    policy =>
    {
        policy.WithOrigins("http://localhost:4004", "http://127.0.0.1:4004").AllowAnyHeader().AllowAnyMethod().AllowCredentials();
    });
});


var app = builder.Build();
app.UseAuthentication();
app.UseAuthorization();
app.MapEndpoints();
app.MapGet("/", () => new { Title = "ClubSync Admin App" });
app.MapGet("/secure", () => new { Title = "ClubSync Admin App" }).RequireAuthorization();
app.UseCors(allowedRequestOrigins);
using (var scope = app.Services.CreateScope())
{
    await SeedRootUser.RunAsync(scope.ServiceProvider);
}
app.Run();

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;

namespace ClubSync.Api.Database.Seeders
{
    public class SeedRootUser : ISeeder
    {
        public static async Task RunAsync(IServiceProvider provider)
        {
            using var scope = provider.CreateScope();
            var scopedProvider = scope.ServiceProvider;

            var dbContext = scopedProvider.GetRequiredService<ClubSyncIdentityDbContext>();
            var userManager = scopedProvider.GetRequiredService<UserManager<User>>();
            var config = scopedProvider.GetRequiredService<IConfiguration>();

            string? email = config["ClubSync:RootUser:Email"] ?? throw new InvalidOperationException("Root User's 'Email' is not defined on configuration.");
            string? password = config["ClubSync:RootUser:Password"] ?? throw new InvalidOperationException("Root User's 'Password' is not defined on configuration.");
            string? givenName = config["ClubSync:RootUser:GivenName"] ?? throw new InvalidOperationException("Root User's 'GivenName' is not defined on configuration.");
            string? surname = config["ClubSync:RootUser:Surname"] ?? throw new InvalidOperationException("Root User's 'Surname' is not defined on configuration.");
            string username = "Owner";

            // Check if user already exists
            if (userManager.Users.Any(u => u.Email == email || u.UserName == username))
            {
                return;
            }

            var user = new User
            {
                GivenName = givenName,
                Surname = surname,
                Email = email,
                NormalizedEmail = email.ToUpper(),
                UserName = username,
                NormalizedUserName = username.ToUpper(),
                EmailConfirmed = true,
                SecurityStamp = Guid.NewGuid().ToString("D")
            };

            // Create the user using UserManager (handles password hashing and validation)
            var result = await userManager.CreateAsync(user, password);

            if (!result.Succeeded)
            {
                throw new Exception($"Failed to create root user: {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
        }
    }
}

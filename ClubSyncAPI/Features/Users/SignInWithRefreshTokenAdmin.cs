using ClubSyncAPI.Database;
using ClubSyncAPI.Endpoints;
using ClubSyncAPI.Features.Users.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace ClubSyncAPI.Features.Users
{
    public class SignInWithRefreshTokenAdmin
    {

        public static async Task<IResult> HandleAsync(
        [FromServices] SignInManager<User> signInManager,
        [FromServices] UserManager<User> userManager,
        ClubSyncIdentityDbContext dbContext,
        [FromServices] TokenProvider tokenProvider,
        [FromServices] ILogger<SignInWithRefreshTokenAdmin> logger,
        HttpContext httpContext)
        {

            if (!httpContext.Request.Cookies.TryGetValue("RefreshToken", out var refreshToken) || string.IsNullOrEmpty(refreshToken))
            {
                logger.LogInformation("Invalid refresh token");
                return Results.BadRequest(new
                {
                    message = "Invalid request.",
                    status = StatusCodes.Status400BadRequest
                });
            }
            var userToken = await dbContext.Set<UserToken>().Where(ut => ut.Value == refreshToken).FirstOrDefaultAsync();
            if (userToken is null)
            {
                logger.LogInformation("Refresh token might be expired or renewed.");
                return Results.BadRequest(new
                {
                    message = "Invalid request.",
                    status = StatusCodes.Status400BadRequest
                });
            }
            var user = await dbContext.Set<User>().Where(u => u.Id == userToken.UserId).FirstOrDefaultAsync();
            if (user is null)
            {
                logger.LogInformation("There is no user associated with this refresh token.");
                return Results.BadRequest(new
                {
                    message = "Invalid request.",
                    status = StatusCodes.Status400BadRequest
                });
            }

            if (!await userManager.VerifyUserTokenAsync(user, "REFRESHTOKENPROVIDER", "RefreshToken", refreshToken))
            {
                logger.LogInformation("Token verification error.");
                return Results.BadRequest(new
                {
                    message = "Invalid request.",
                    status = StatusCodes.Status400BadRequest
                });
            }
            var token = tokenProvider.GenerateToken(user);
            await userManager.RemoveAuthenticationTokenAsync(user, "REFRESHTOKENPROVIDER", "RefreshToken");
            var newRefreshToken = await userManager.GenerateUserTokenAsync(user, "REFRESHTOKENPROVIDER", "RefreshToken");
            await userManager.SetAuthenticationTokenAsync(user, "REFRESHTOKENPROVIDER", "RefreshToken", newRefreshToken);
            httpContext.Response.Cookies.Append("RefreshToken", newRefreshToken, new CookieOptions
            {
                HttpOnly = true, // Prevent JavaScript access
                Secure = true, // Ensure it's sent over HTTPS
                SameSite = SameSiteMode.Lax,
                Expires = DateTime.UtcNow.AddDays(1), // Set expiration

            });

            var result = await userManager.GetAuthenticationTokenAsync(user, "REFRESHTOKENPROVIDER", "RefreshToken");
            return Results.Ok(new
            {
                message = "OK",
                status = StatusCodes.Status200OK,
                accessToken = token,
                refreshToken,
                user = new
                {
                    id = user.Id,
                    givenName = user.GivenName,
                    surname = user.Surname,
                    email = user.Email,
                }
            });
        }
        internal sealed class Endpoint : IEndpoint
        {
            public static void Map(IEndpointRouteBuilder app)
            {
                app.MapPost("/api/admin/v1/refresh-token", HandleAsync);
            }
        }

    }

}
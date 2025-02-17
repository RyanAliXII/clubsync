using ClubSync.Api.Database;
using ClubSync.Api.Endpoints;
using ClubSync.Api.Features.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
namespace ClubSync.Api.Features.Users
{
    public class SignInAdminAccountUsingRefreshToken
    {
        public static async Task<IResult> HandleAsync(
        [FromServices] UserManager<User> userManager,
        ClubSyncIdentityDbContext dbContext,
        [FromServices] TokenProvider tokenProvider,
        [FromServices] ILogger<SignInAdminAccountUsingRefreshToken> logger,
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
            var userToken = await dbContext.UserTokens.Where(ut => ut.Value == refreshToken).FirstOrDefaultAsync();
            if (userToken is null)
            {
                logger.LogInformation("Refresh token might be expired or renewed.");
                return Results.BadRequest(new
                {
                    message = "Invalid request.",
                    status = StatusCodes.Status400BadRequest
                });
            }
            var user = await dbContext.Users.Where(u => u.Id == userToken.UserId).FirstOrDefaultAsync();
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
using ClubSync.Api.Database;
using ClubSync.Api.Endpoints;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
namespace ClubSync.Api.Features.Users
{
    public class SignInAdminAccount
    {
        public record Request(string Email, string Password);
        public static async Task<IResult> HandleAsync(
        [FromServices] SignInManager<User> signInManager,
        [FromServices] UserManager<User> userManager,
        [FromServices] TokenProvider tokenProvider,
        HttpContext httpContext,
        [FromBody] Request request)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                return Results.BadRequest(new
                {
                    message = "Invalid email or password.",
                    status = StatusCodes.Status400BadRequest
                });
            }
            var result = await signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!result.Succeeded)
            {
                return Results.BadRequest(new
                {
                    message = "Invalid email or password.",
                    status = StatusCodes.Status400BadRequest
                });
            }
            var token = tokenProvider.GenerateToken(user);
            await userManager.RemoveAuthenticationTokenAsync(user, "REFRESHTOKENPROVIDER", "RefreshToken");
            var refreshToken = await userManager.GenerateUserTokenAsync(user, "REFRESHTOKENPROVIDER", "RefreshToken");
            await userManager.SetAuthenticationTokenAsync(user, "REFRESHTOKENPROVIDER", "RefreshToken", refreshToken);
            httpContext.Response.Cookies.Append("RefreshToken", refreshToken, new CookieOptions
            {
                HttpOnly = true, // Prevent JavaScript access
                Secure = true, // Ensure it's sent over HTTPS
                SameSite = SameSiteMode.Lax,
                Expires = DateTime.UtcNow.AddDays(1), // Set expiration

            });
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
                app.MapPost("/api/admin/v1/sign-in", HandleAsync);
            }
        }

    }

}
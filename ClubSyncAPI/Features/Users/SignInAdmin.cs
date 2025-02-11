using ClubSyncAPI.Database;
using ClubSyncAPI.Endpoints;
using ClubSyncAPI.Features.Users.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
namespace ClubSyncAPI.Features.Users
{
    public class SignInAdmin
    {
        public record Request(string Email, string Password);
        public static async Task<IResult> HandleAsync(
        [FromServices] SignInManager<User> signInManager,
        [FromServices] UserManager<User> userManager,
        [FromServices] TokenProvider tokenProvider,
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

            return Results.Ok(new
            {
                message = "OK",
                status = StatusCodes.Status200OK,
                accessToken = token,
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
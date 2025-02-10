using ClubSyncAPI.Endpoints;
namespace ClubSyncAPI.Features.Auth
{
    public class AuthenticateAdmin
    {
        public record Request(string Email, string Password);
        public static async Task<IResult> HandleAsync()
        {
            return Results.Ok();
        }
        internal sealed class Endpoint : IEndpoint
        {
            public static void Map(IEndpointRouteBuilder app)
            {
                app.MapPost("/api/admin/v1/authenticate", HandleAsync);
            }
        }

    }

}
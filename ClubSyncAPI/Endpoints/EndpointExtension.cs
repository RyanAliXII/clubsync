using ClubSyncAPI.Features.Users;
namespace ClubSyncAPI.Endpoints
{
    public static class EndpointExtension
    {
        public static void MapEndpoints(this IEndpointRouteBuilder app)
        {
            SignInAdmin.Endpoint.Map(app);
            SignInWithRefreshTokenAdmin.Endpoint.Map(app);
        }
    }

}
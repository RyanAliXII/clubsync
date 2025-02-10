using ClubSyncAPI.Features.Auth;

namespace ClubSyncAPI.Endpoints
{
    public static class EndpointExtension
    {
        public static void MapEndpoints(this IEndpointRouteBuilder app)
        {
            AuthenticateAdmin.Endpoint.Map(app);
        }
    }

}
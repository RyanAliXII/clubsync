using ClubSync.Api.Features.Users;
namespace ClubSync.Api.Endpoints
{
    public static class EndpointExtension
    {
        public static void MapEndpoints(this IEndpointRouteBuilder app)
        {
            SignInAdminAccount.Endpoint.Map(app);
            SignInAdminAccountUsingRefreshToken.Endpoint.Map(app);
        }
    }

}